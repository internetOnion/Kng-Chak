using System;
using KngChak.Data;
using KngChak.Flow;
using UnityEngine;

namespace KngChak.Rocks
{
    /// <summary>
    /// A falling rock. Reads its size and tint from a RockTypeDefinition,
    /// and can be primed and broken once each.
    /// Also raises GameEvents.RockPrimed and GameEvents.RockBroken.
    /// </summary>
    public class Rock : MonoBehaviour
    {
        [SerializeField] private RockTypeDefinition _rockType;

        private SpriteRenderer _sr;

        /// <summary>The data this rock was made from (fall speed, damage, score...).</summary>
        public RockTypeDefinition RockType => _rockType;

        public bool IsPrimed { get; private set; }
        public bool IsBroken { get; private set; }

        /// <summary>Raised once, the first time the rock is primed.</summary>
        public event Action Primed;

        /// <summary>Raised once, when the rock is broken.</summary>
        public event Action Broken;

        public void Prime()
        {
            if (IsPrimed || IsBroken) return;
            IsPrimed = true;
            Primed?.Invoke();
            GameEvents.RockPrimed();
        }

        public void Break()
        {
            if (IsBroken) return;
            IsBroken = true;
            Broken?.Invoke();
            GameEvents.RockBroken();
        }

        void Awake()
        {
            ApplyDefinition();
        }

        // Runs when a value is changed in the Inspector: preview the rock type's size and tint without pressing Play.
        void OnValidate()
        {
            ApplyDefinition();
        }

        private void ApplyDefinition()
        {
            if (_rockType == null) return;
            if (_sr == null) _sr = GetComponent<SpriteRenderer>();
            _sr.color = _rockType.Tint;
            transform.localScale = Vector3.one * _rockType.Size;
        }
    }
}
