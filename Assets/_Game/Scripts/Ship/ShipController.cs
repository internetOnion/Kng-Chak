using System;
using UnityEngine;

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

        private float _throttle;
        /// <summary>
        /// tell how hard the engine is pushing the ship the range is between 0 and 1
        /// </summary>
        public float Throttle
        {
            get => _throttle;
            private set => _throttle = Mathf.Clamp01(value);
        }

        /// <summary>
        /// True while a boost burst is active
        /// </summary>
        public bool IsBoosting { get; private set; }

        /// <summary>
        /// Raised after the ship is bumped. The value is the impulse:
        /// the direction the ship is pushed, with its length as the strength.
        /// </summary>
        public event Action<Vector2> Bumped;

        /// <summary>
        /// Bumps the ship away with an impulse (direction × strength),
        /// for example when a rock hits its back half.
        /// </summary>
        public void Bump(Vector2 impulse)
        {
            // TODO(#10): push the ship away using impulse
            Bumped?.Invoke(impulse);
        }
    }
}
