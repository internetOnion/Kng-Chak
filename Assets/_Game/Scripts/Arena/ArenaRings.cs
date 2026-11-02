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

        // radii last drawn, so circles are only rebuilt when the config changes (-1 = not drawn yet)
        private float _drawnEdgeRadius = -1f;
        private float _drawnOrbitRadius = -1f;


        // ── Lifecycle ────────────────────────────────────────────────
        void OnEnable()
        {
            _edgeRing = GetOrCreate("EdgeRing");
            _orbitRing = GetOrCreate("OrbitRing");
            _drawnEdgeRadius = -1f;
            _drawnOrbitRadius = -1f;
        }

        void Update()
        {
            if (_config == null) return;

            if (_config.ArenaRadius != _drawnEdgeRadius)
            {
                DrawCircle(_edgeRing, _config.ArenaRadius);   // big outer edge
                _drawnEdgeRadius = _config.ArenaRadius;
            }
            if (_config.OrbitRadius != _drawnOrbitRadius)
            {
                DrawCircle(_orbitRing, _config.OrbitRadius);   // inner orbit band
                _drawnOrbitRadius = _config.OrbitRadius;
            }

            FadeEdge();


        }


        // ── Circle drawing ───────────────────────────────────────────
        /// <summary>Writes SEGMENTS local positions around a circle (moving this object moves the rings).</summary>
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
                lr.SetPosition(i, new Vector3(x, y, 0f));
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
            lr.useWorldSpace = false; // positions are relative to this object, so they only need drawing once
            lr.loop = true;
            // closes the circle

            lr.startWidth = 0.15f;
            lr.endWidth = 0.15f;

            // create the material once: with [ExecuteAlways], OnEnable runs again and again in the editor
            if (lr.sharedMaterial == null)
                lr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));


            return lr;
        }



    }



}
