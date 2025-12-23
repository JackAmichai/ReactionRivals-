using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace ReactionRivals
{
    /// <summary>
    /// Animated visualization of an atom with orbiting electrons.
    /// Shows protons/neutrons in nucleus and electrons in shells.
    /// </summary>
    public class AtomVisualization : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform nucleusArea;
        [SerializeField] private RectTransform electronOrbitArea;

        [Header("Prefabs")]
        [SerializeField] private GameObject protonPrefab;
        [SerializeField] private GameObject neutronPrefab;
        [SerializeField] private GameObject electronPrefab;
        [SerializeField] private GameObject orbitRingPrefab;

        [Header("Colors")]
        [SerializeField] private Color protonColor = new Color(1f, 0.3f, 0.3f, 1f);
        [SerializeField] private Color neutronColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        [SerializeField] private Color electronColor = new Color(0.3f, 0.5f, 1f, 1f);
        [SerializeField] private Color orbitColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);

        [Header("Sizing")]
        [SerializeField] private float nucleusRadius = 50f;
        [SerializeField] private float particleSize = 20f;
        [SerializeField] private float electronSize = 12f;
        [SerializeField] private float orbitStartRadius = 80f;
        [SerializeField] private float orbitSpacing = 40f;

        [Header("Animation")]
        [SerializeField] private float electronOrbitSpeed = 50f;
        [SerializeField] private bool animateElectrons = true;

        // Current state
        private int currentProtons = 0;
        private int currentNeutrons = 0;
        private int currentElectrons = 0;

        // Visual elements
        private System.Collections.Generic.List<GameObject> protonVisuals = new System.Collections.Generic.List<GameObject>();
        private System.Collections.Generic.List<GameObject> neutronVisuals = new System.Collections.Generic.List<GameObject>();
        private System.Collections.Generic.List<GameObject> electronVisuals = new System.Collections.Generic.List<GameObject>();
        private System.Collections.Generic.List<GameObject> orbitRings = new System.Collections.Generic.List<GameObject>();

        // Electron orbit data
        private System.Collections.Generic.List<float> electronAngles = new System.Collections.Generic.List<float>();
        private System.Collections.Generic.List<int> electronShellIndex = new System.Collections.Generic.List<int>();

        private void Update()
        {
            if (animateElectrons)
            {
                AnimateElectrons();
            }
        }

        public void SetAtom(int protons, int neutrons, int electrons)
        {
            currentProtons = protons;
            currentNeutrons = neutrons;
            currentElectrons = electrons;
            RebuildVisualization();
        }

        public void UpdateProtons(int count)
        {
            currentProtons = count;
            UpdateNucleusVisuals();
        }

        public void UpdateNeutrons(int count)
        {
            currentNeutrons = count;
            UpdateNucleusVisuals();
        }

        public void UpdateElectrons(int count)
        {
            currentElectrons = count;
            UpdateElectronVisuals();
        }

        public void Clear()
        {
            ClearAllVisuals();
            currentProtons = 0;
            currentNeutrons = 0;
            currentElectrons = 0;
        }

        private void ClearAllVisuals()
        {
            foreach (var p in protonVisuals) if (p) Destroy(p);
            foreach (var n in neutronVisuals) if (n) Destroy(n);
            foreach (var e in electronVisuals) if (e) Destroy(e);
            foreach (var o in orbitRings) if (o) Destroy(o);

            protonVisuals.Clear();
            neutronVisuals.Clear();
            electronVisuals.Clear();
            orbitRings.Clear();
            electronAngles.Clear();
            electronShellIndex.Clear();
        }

        private void RebuildVisualization()
        {
            ClearAllVisuals();
            UpdateNucleusVisuals();
            UpdateElectronVisuals();
        }

        private void UpdateNucleusVisuals()
        {
            Transform parent = nucleusArea != null ? nucleusArea : transform;

            // Update protons
            while (protonVisuals.Count < currentProtons)
            {
                var proton = CreateParticle(protonPrefab, protonColor, particleSize, parent);
                protonVisuals.Add(proton);
            }
            while (protonVisuals.Count > currentProtons)
            {
                Destroy(protonVisuals[protonVisuals.Count - 1]);
                protonVisuals.RemoveAt(protonVisuals.Count - 1);
            }

            // Update neutrons
            while (neutronVisuals.Count < currentNeutrons)
            {
                var neutron = CreateParticle(neutronPrefab, neutronColor, particleSize, parent);
                neutronVisuals.Add(neutron);
            }
            while (neutronVisuals.Count > currentNeutrons)
            {
                Destroy(neutronVisuals[neutronVisuals.Count - 1]);
                neutronVisuals.RemoveAt(neutronVisuals.Count - 1);
            }

            // Position all nucleus particles
            PositionNucleusParticles();
        }

        private void PositionNucleusParticles()
        {
            int totalParticles = protonVisuals.Count + neutronVisuals.Count;
            if (totalParticles == 0) return;

            // Combine into single list for positioning
            var allParticles = new System.Collections.Generic.List<GameObject>();
            
            // Interleave protons and neutrons for realistic look
            int pIndex = 0, nIndex = 0;
            while (pIndex < protonVisuals.Count || nIndex < neutronVisuals.Count)
            {
                if (pIndex < protonVisuals.Count)
                    allParticles.Add(protonVisuals[pIndex++]);
                if (nIndex < neutronVisuals.Count)
                    allParticles.Add(neutronVisuals[nIndex++]);
            }

            // Position in a packed cluster
            for (int i = 0; i < allParticles.Count; i++)
            {
                Vector2 pos = GetNucleusPosition(i, allParticles.Count);
                RectTransform rt = allParticles[i].GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = pos;
                }
                else
                {
                    allParticles[i].transform.localPosition = new Vector3(pos.x, pos.y, 0);
                }
            }
        }

        private Vector2 GetNucleusPosition(int index, int total)
        {
            if (total <= 1) return Vector2.zero;

            // Use golden angle spiral for natural-looking packing
            float goldenAngle = 137.5f * Mathf.Deg2Rad;
            float angle = index * goldenAngle;
            
            // Radius grows with sqrt for even distribution
            float maxRadius = nucleusRadius * Mathf.Min(1f, Mathf.Sqrt(total) * 0.2f);
            float radius = maxRadius * Mathf.Sqrt((float)index / total);

            return new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );
        }

        private void UpdateElectronVisuals()
        {
            Transform parent = electronOrbitArea != null ? electronOrbitArea : transform;

            // Calculate shell distribution
            int[] shellCapacity = { 2, 8, 18, 32, 32, 18, 8 };
            int[] electronsPerShell = new int[7];
            int remaining = currentElectrons;

            for (int i = 0; i < 7 && remaining > 0; i++)
            {
                electronsPerShell[i] = Mathf.Min(remaining, shellCapacity[i]);
                remaining -= electronsPerShell[i];
            }

            // Count needed shells
            int shellsNeeded = 0;
            for (int i = 0; i < 7; i++)
            {
                if (electronsPerShell[i] > 0) shellsNeeded = i + 1;
            }

            // Update orbit rings
            while (orbitRings.Count < shellsNeeded)
            {
                var ring = CreateOrbitRing(orbitStartRadius + orbitRings.Count * orbitSpacing, parent);
                orbitRings.Add(ring);
            }
            while (orbitRings.Count > shellsNeeded)
            {
                Destroy(orbitRings[orbitRings.Count - 1]);
                orbitRings.RemoveAt(orbitRings.Count - 1);
            }

            // Update electrons
            while (electronVisuals.Count < currentElectrons)
            {
                var electron = CreateParticle(electronPrefab, electronColor, electronSize, parent);
                electronVisuals.Add(electron);
                electronAngles.Add(Random.Range(0f, 360f));
                electronShellIndex.Add(0);
            }
            while (electronVisuals.Count > currentElectrons)
            {
                int lastIndex = electronVisuals.Count - 1;
                Destroy(electronVisuals[lastIndex]);
                electronVisuals.RemoveAt(lastIndex);
                electronAngles.RemoveAt(lastIndex);
                electronShellIndex.RemoveAt(lastIndex);
            }

            // Assign electrons to shells
            int electronIndex = 0;
            for (int shell = 0; shell < 7 && electronIndex < electronVisuals.Count; shell++)
            {
                int electronsInThisShell = electronsPerShell[shell];
                for (int e = 0; e < electronsInThisShell && electronIndex < electronVisuals.Count; e++)
                {
                    electronShellIndex[electronIndex] = shell;
                    // Space electrons evenly in shell
                    electronAngles[electronIndex] = (360f / electronsInThisShell) * e;
                    electronIndex++;
                }
            }

            PositionElectrons();
        }

        private void PositionElectrons()
        {
            for (int i = 0; i < electronVisuals.Count; i++)
            {
                int shell = electronShellIndex[i];
                float radius = orbitStartRadius + shell * orbitSpacing;
                float angle = electronAngles[i] * Mathf.Deg2Rad;

                Vector2 pos = new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius
                );

                RectTransform rt = electronVisuals[i].GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = pos;
                }
                else
                {
                    electronVisuals[i].transform.localPosition = new Vector3(pos.x, pos.y, 0);
                }
            }
        }

        private void AnimateElectrons()
        {
            for (int i = 0; i < electronAngles.Count; i++)
            {
                // Outer shells orbit slower
                int shell = electronShellIndex[i];
                float speedModifier = 1f / (1f + shell * 0.3f);
                electronAngles[i] += electronOrbitSpeed * speedModifier * Time.deltaTime;
                
                if (electronAngles[i] > 360f)
                    electronAngles[i] -= 360f;
            }
            PositionElectrons();
        }

        private GameObject CreateParticle(GameObject prefab, Color color, float size, Transform parent)
        {
            GameObject particle;
            
            if (prefab != null)
            {
                particle = Instantiate(prefab, parent);
            }
            else
            {
                // Create UI particle
                particle = new GameObject("Particle");
                particle.transform.SetParent(parent);
                
                var image = particle.AddComponent<Image>();
                image.color = color;
                
                // Make it circular
                image.sprite = CreateCircleSprite();
            }

            var rectTransform = particle.GetComponent<RectTransform>();
            if (rectTransform == null)
                rectTransform = particle.AddComponent<RectTransform>();
            
            rectTransform.sizeDelta = new Vector2(size, size);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;

            // Set color if it has an Image component
            var img = particle.GetComponent<Image>();
            if (img != null)
                img.color = color;

            return particle;
        }

        private GameObject CreateOrbitRing(float radius, Transform parent)
        {
            GameObject ring;
            
            if (orbitRingPrefab != null)
            {
                ring = Instantiate(orbitRingPrefab, parent);
            }
            else
            {
                ring = new GameObject("OrbitRing");
                ring.transform.SetParent(parent);
                
                var image = ring.AddComponent<Image>();
                image.color = orbitColor;
                image.sprite = CreateRingSprite();
                image.type = Image.Type.Simple;
                image.preserveAspect = true;
            }

            var rt = ring.GetComponent<RectTransform>();
            if (rt == null) rt = ring.AddComponent<RectTransform>();
            
            rt.sizeDelta = new Vector2(radius * 2, radius * 2);
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            // Make sure ring is behind electrons
            ring.transform.SetAsFirstSibling();

            return ring;
        }

        private Sprite CreateCircleSprite()
        {
            int size = 64;
            Texture2D texture = new Texture2D(size, size);
            Color[] colors = new Color[size * size];
            
            float center = size / 2f;
            float radius = size / 2f - 1;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    if (dist < radius)
                        colors[y * size + x] = Color.white;
                    else
                        colors[y * size + x] = Color.clear;
                }
            }

            texture.SetPixels(colors);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        private Sprite CreateRingSprite()
        {
            int size = 128;
            Texture2D texture = new Texture2D(size, size);
            Color[] colors = new Color[size * size];
            
            float center = size / 2f;
            float outerRadius = size / 2f - 1;
            float innerRadius = outerRadius - 2;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    if (dist < outerRadius && dist > innerRadius)
                        colors[y * size + x] = Color.white;
                    else
                        colors[y * size + x] = Color.clear;
                }
            }

            texture.SetPixels(colors);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        // Quick setup method
        public void SetupForElement(ElementBuildData element)
        {
            if (element == null) return;
            
            // Create orbit rings for expected shells
            int expectedShells = element.ElectronShells?.Length ?? 1;
            Transform parent = electronOrbitArea != null ? electronOrbitArea : transform;
            
            // Clear and create expected orbit rings
            foreach (var o in orbitRings) if (o) Destroy(o);
            orbitRings.Clear();
            
            for (int i = 0; i < expectedShells; i++)
            {
                var ring = CreateOrbitRing(orbitStartRadius + i * orbitSpacing, parent);
                // Make unfilled shells more transparent
                var img = ring.GetComponent<Image>();
                if (img != null)
                {
                    Color c = orbitColor;
                    c.a = 0.15f;
                    img.color = c;
                }
                orbitRings.Add(ring);
            }
        }
    }
}
