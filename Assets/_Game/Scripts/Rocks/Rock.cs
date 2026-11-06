using System;
using KngChak.Data;
using KngChak.Flow;
using KngChak.Combat;
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
        [SerializeField] private ArenaConfig _arena;

        private PlanetHealth _planet;

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



        void Start()
        {
            _planet = FindAnyObjectByType<PlanetHealth>();
            if (_planet == null) Debug.Log("Rock : No planet component in the scene", this);
        }
        void Awake()
        {
            ApplyDefinition();


        }
        void Update()
        {
            if (_planet == null || _arena == null || _rockType == null || IsBroken) return;

            Vector2 planetPos = _planet.transform.position;
            Vector2 newPos = Vector2.MoveTowards(
                transform.position, planetPos, _rockType.FallSpeed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            float hitDistance = _arena.PlanetRadius + _rockType.Size;
            if (Vector2.Distance(newPos, planetPos) <= hitDistance)
                HitPlanet();
        }

        void HitPlanet()
        {
            _planet.TakeDamage(_rockType.DamagePercent);
            Destroy(gameObject);
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

            // Size is a radius: scale the sprite so its width = 2 × Size, whatever the sprite's own size
            float spriteWidth = _sr.sprite != null ? _sr.sprite.bounds.size.x : 1f;
            transform.localScale = Vector3.one * (2f * _rockType.Size / spriteWidth);
        }
    }
}
