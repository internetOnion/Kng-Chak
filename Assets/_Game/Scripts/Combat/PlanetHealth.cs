using System;
using KngChak.Flow;
using UnityEngine;

namespace KngChak.Combat
{
    /// <summary>
    /// Tracks the planet's health as an integer percentage (0–100).
    /// Lives on the Planet prefab. Also raises GameEvents.PlanetHit and GameEvents.GameOver.
    /// </summary>
    public class PlanetHealth : MonoBehaviour
    {
        private const int MaxHP = 100; // HP is a percentage

        /// <summary>Current HP as a percentage (0–100).</summary>
        public int HP { get; private set; } = MaxHP;

        /// <summary>True once HP has reached 0.</summary>
        public bool IsDead => HP <= 0;

        /// <summary>
        /// Raised whenever HP changes.
        /// Parameter: new HP value (0–100).
        /// </summary>
        public event Action<int> HPChanged;

        /// <summary>
        /// Raised once when HP reaches 0.
        /// </summary>
        public event Action Died;

        /// <summary>
        /// Reduces HP by <paramref name="amount"/> percent.
        /// Clamps to 0 and fires <see cref="Died"/> on the killing hit.
        /// Ignores amounts of 0 or less.
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (IsDead || amount <= 0) return;
            HP = Mathf.Max(HP - amount, 0);
            HPChanged?.Invoke(HP);
            GameEvents.PlanetHit(amount);
            if (IsDead)
            {
                Died?.Invoke();
                GameEvents.GameOver();
            }
        }

        /// <summary>
        /// Restores HP by <paramref name="amount"/> percent.
        /// Clamps to 100. Does nothing if already dead, already full, or for amounts of 0 or less.
        /// </summary>
        public void Heal(int amount)
        {
            if (IsDead || amount <= 0 || HP == MaxHP) return;
            HP = Mathf.Min(HP + amount, MaxHP);
            HPChanged?.Invoke(HP);
        }
    }
}
