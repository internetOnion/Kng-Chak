using UnityEngine;
using KngChak.Data;
using System;

namespace KngChak.Arena
{
    [ExecuteAlways] // runs in Edit mode — radius changes update live
    public class ArenaRings : MonoBehaviour
    {

        [Header("Config")]
        [SerializeField] private ArenaConfig _config;

        [Header("Ship(for edge fade)")]
        [SerializeField] private Transform _ship;
        // assign the player/ship transform


        // LineRenderers are created automatically as child objects
        private LineRenderer _edgeRing;
        private LineRenderer _orbitRing;


        private const int SEGMENTS = 128; // how smooth the circle looks


        // ── Lifecycle ──────────────────────────────────────────────── 
        void OnEnable()
        {
            _edgeRing = GetOrCreate("EdgeRing");
            _orbitRing = GetOrCreate("OrbitRing");
        }

        void Update()
        {
            if (_config == null) return;
            DrawCircle(_edgeRing, _config.ArenaRadius);   // big outer edge
            DrawCircle(_orbitRing, _config.OrbitRadius);   // inner orbit band

            FadeEdge();


        }


        // ── Circle drawing ───────────────────────────────────────────
        /// <summary>Writes SEGMENTS world-space positions around a circle.</summary>
        void DrawCircle(LineRenderer lr, float radius)
        {
            if (lr == null) return;
            lr.positionCount = SEGMENTS;


            for (int i = 0; i < SEGMENTS; i++)
            {
                float angle = (float)i / SEGMENTS * MathF.PI * 2f;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                // 2D game → positions are on the XY plane
                lr.SetPosition(i, transform.position + new Vector3(x, y, 0f));
            }


        }


        // ── Edge fade ────────────────────────────────────────────────
        /// <summary>
        /// Fades the edge ring's alpha in as the ship approaches within EdgeShowDist.
        /// Fully invisible when far, fully visible at the boundary.
        /// </summary>
        void FadeEdge()
        {
            if (_edgeRing == null || _config == null) return;
            float alpha = 0f;
            if (_ship != null)
            {
                float distFromCenter = Vector3.Distance(_ship.position, transform.position);
                float distFromEdge = _config.ArenaRadius - distFromCenter;

                // Fade in as distFromEdge shrinks toward 0
                alpha = Mathf.Clamp01(1f - distFromEdge / _config.EdgeShowDist);

                Color c = _edgeRing.startColor;
                c.a = alpha;
                _edgeRing.startColor = c;
                _edgeRing.endColor = c;
            }
        }


        // ── Helper: get or create a LineRenderer child ────────────────
        LineRenderer GetOrCreate(string childName)
        {
            Transform t = transform.Find(childName);
            if (t == null)
            {
                t = new GameObject(childName).transform;
                t.SetParent(transform, false);
            }

            var lr = t.GetComponent<LineRenderer>();
            if (lr == null) lr = t.gameObject.AddComponent<LineRenderer>();
            lr.sortingLayerName = "ArenaFX";


            // ← the key sorting layer
            lr.useWorldSpace = true;
            lr.loop = true;
            // closes the circle

            lr.startWidth = 0.15f;
            lr.endWidth = 0.15f;
            lr.material = new Material(Shader.Find("Sprites/Default"));


            return lr;
        }



    }



}
