using System;
using UnityEngine;
using UnityEngine.InputSystem;
using KngChak.Data;

namespace KngChak.Ship
{
    public class ShipController : MonoBehaviour
    {
        /// <summary>
        /// tell the direction and how fast the ship going in world units per second
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        ///  the direction that the head of the ship pointing to
        /// transform.up always has length 1, and with 2D rotation only around z, dropping z keeps it at length 1
        /// </summary>
        public Vector2 Facing => transform.up;

        /// <summary>
        /// True while a boost burst is active
        /// </summary>
        public bool IsBoosting { get; private set; }

        /// <summary>
        /// Raised after the ship is bumped. The value is the impulse:
        /// the direction the ship is pushed, with its length as the strength.
        /// </summary>
        public event Action<Vector2> Bumped;

        [SerializeField] private ShipConfig _config;

        [SerializeField] private ArenaConfig _arena;

        private Camera _cam;

        private float _throttle;

        private float _boostTimeLeft;   // counts down while boosting; ≤ 0 = not boosting

        private void Awake() => _cam = Camera.main;

        private void Update()
        {
            float dt = Time.deltaTime;
            bool hasWant = TryReadCursor(out Vector2 want, out float wantThrottle);
            ReadButtons(out bool boostPressed, out bool braking);

            // turn the nose toward the cursor
            if (hasWant)
            {
                float turn = Vector2.SignedAngle(Facing, want);
                float maxStep = _config.TurnRate * dt;
                transform.Rotate(0f, 0f, Mathf.Clamp(turn, -maxStep, maxStep));
            }

            // boost: a tap starts a short burst along the nose
            if (boostPressed && !IsBoosting)
            {
                _boostTimeLeft = _config.BoostDuration;
                if (hasWant)   // boost = instant sharp turn: fire where you aim, not where the nose was
                    transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, want));
            }
            IsBoosting = _boostTimeLeft > 0f;

            if (IsBoosting)
            {
                Throttle = 1f;                                   // engine flat out
                Velocity = Facing * _config.BoostSpeed;          // steers with the nose
                _boostTimeLeft -= dt;
                if (_boostTimeLeft <= 0f)
                    Velocity = Velocity.normalized * _config.MaxSpeed;   // boost over: back to cruise
            }
            else
            {
                // thrust, only once the nose is roughly pointing the right way
                bool thrust = hasWant && wantThrottle > 0.02f
                    && Mathf.Abs(Vector2.SignedAngle(Facing, want)) < _config.ThrustConeDeg;
                Throttle = thrust ? wantThrottle : 0f; // the flame push zone (#19) will read this

                float cap = _config.MaxSpeed * Mathf.Max(wantThrottle, 0.15f); // speed the cursor asks for
                float speed = Velocity.magnitude;
                if (thrust && speed < cap)
                    Velocity += Facing * Mathf.Min(_config.ThrustAccel * dt, cap - speed);

                // grip: bend momentum toward the nose (same speed, new direction)
                if (hasWant && Velocity.sqrMagnitude > 0.0001f)
                {
                    Vector2 alongNose = Facing * Velocity.magnitude;
                    float t = Mathf.Min(1f, _config.Grip * (thrust ? 1.6f : 1f) * dt);
                    Velocity = Vector2.Lerp(Velocity, alongNose, t);
                }

                // drag: braking > coasting > thrusting
                float damp = braking ? _config.Brake : _config.Drag * (thrust ? 0.2f : 1f);
                Velocity *= Mathf.Max(0f, 1f - damp * dt);
            }

            // move, then keep the ship inside the ring
            Vector2 pos = (Vector2)transform.position + Velocity * dt;
            KeepInArena(ref pos);
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }

        // Editor-only: show the arena edge and orbit track when the ship is selected.
        private void OnDrawGizmosSelected()
        {
            if (_arena == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Vector3.zero, _arena.ArenaRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, _arena.OrbitRadius);
        }

        /// <summary>
        /// tell how hard the engine is pushing the ship the range is between 0 and 1
        /// </summary>
        public float Throttle
        {
            get => _throttle;
            private set => _throttle = Mathf.Clamp01(value);
        }

        /// <summary>
        /// Bumps the ship away with an impulse (direction × strength),
        /// for example when a rock hits its back half.
        /// </summary>
        public void Bump(Vector2 impulse)
        {
            _boostTimeLeft = 0f; // a bump cuts a boost short, or the boost would overwrite the shove next frame
            IsBoosting = false;
            Velocity += impulse;
            Bumped?.Invoke(impulse);
        }

        // Where the cursor wants the ship to go, and how hard (0–1).
        // Returns false when there's no mouse or the cursor is on top of the ship.
        private bool TryReadCursor(out Vector2 want, out float throttle)
        {
            want = default;
            throttle = 0f;

            var mouse = Mouse.current;
            if (mouse == null) return false;

            Vector2 cursor = _cam.ScreenToWorldPoint(mouse.position.ReadValue());
            Vector2 toCursor = cursor - (Vector2)transform.position;
            float dist = toCursor.magnitude;
            if (dist < _config.CursorDeadzone) return false;

            want = toCursor / dist; // length 1: direction only
            throttle = Mathf.Clamp01((dist - _config.CursorDeadzone) / _config.CursorFullRange);
            return true;
        }

        // Left click = boost (a tap), right click = brake (hold).
        private void ReadButtons(out bool boostPressed, out bool braking)
        {
            var mouse = Mouse.current;
            boostPressed = mouse != null && mouse.leftButton.wasPressedThisFrame;
            braking = mouse != null && mouse.rightButton.isPressed;
        }

        // Slide along the arena edge (no bounce), and never go below the orbit track.
        private void KeepInArena(ref Vector2 pos)
        {
            float r = pos.magnitude; // distance from the planet at (0,0)
            if (r < 0.0001f) return; // dead centre: no outward direction
            Vector2 normal = pos / r; // points from planet to ship, length 1

            if (r > _arena.ArenaRadius)
            {
                pos = normal * _arena.ArenaRadius; // pull back onto the edge
                float outward = Vector2.Dot(Velocity, normal);
                if (outward > 0f) Velocity -= normal * outward;
            }
            else if (r < _arena.OrbitRadius)
            {
                pos = normal * _arena.OrbitRadius; // can't go below the orbit track
                float inward = Vector2.Dot(Velocity, normal);
                if (inward < 0f) Velocity -= normal * inward;
            }
        }
    }
}
