using KngChak.Data;
using KngChak.Ship;
using UnityEngine;

namespace KngChak.Arena
{
    [RequireComponent(typeof(Camera))]
    public class ArenaCamera : MonoBehaviour
    {
        [SerializeField] private ArenaConfig _arena;
        [SerializeField] private ShipController _ship;

        private Camera _cam;

        private void Awake() => _cam = GetComponent<Camera>();

        private void LateUpdate()
        {
            // CamView = world units from the screen centre to the NEARER edge, on any screen shape
            _cam.orthographicSize = _arena.CamView / Mathf.Min(1f, _cam.aspect);

            // how far toward the ship to look: 0 = on the planet, 1 = on the ship
            Vector2 shipPos = _ship.transform.position;
            float u = Mathf.InverseLerp(_arena.CamNear, _arena.CamFar, shipPos.magnitude);
            float w = Mathf.SmoothStep(0f, 1f, u);
            Vector2 look = shipPos * w;

            // leash: keep at least CamPlanetShow of the planet's width on screen
            float halfH = _cam.orthographicSize;
            float halfW = halfH * _cam.aspect;
            float r = _arena.PlanetRadius;
            float keep = 2f * r * _arena.CamPlanetShow;   // planet width that must stay visible
            look.x = Mathf.Clamp(look.x, -(halfW + r - keep), halfW + r - keep);
            look.y = Mathf.Clamp(look.y, -(halfH + r - keep), halfH + r - keep);

            transform.position = new Vector3(look.x, look.y, transform.position.z); // pan only: no rotation, keep the camera's z
        }
    }
}