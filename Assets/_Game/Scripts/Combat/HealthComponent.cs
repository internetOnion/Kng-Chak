using System;
using UnityEngine;

namespace KngChak.Combat
{

    /// <summary>
    /// Tracks ship health as an integer percentage (0–100).
    /// Attach to any GameObject that can take damage.
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        // ── Public read-only state ──────────────────────────────────────

        /// <summary>Current HP as a percentage (0–100).</summary>
        public int HP { get; private set; } = 100;

        /// <summary>True once HP has reached 0.</summary>
        public bool IsDestroy => HP <= 0;


        // ── Events ──────────────────────────────────────────────────────

        /// <summary>
        /// Raised whenever HP changes.
        /// Parameter: new HP value (0–100).
        /// </summary>
        public event Action<int> HPUpdate;
        /// <summary>
        /// Raised once when HP reaches 0.
        /// </summary>
        /// 
        public event Action Destroy;

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>
        /// Reduces HP by <paramref name="amount"/> percent.
        /// Clamps to 0 and fires <see cref="Destory"/> on the killing hit.
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (IsDestroy) return;
            HP = Mathf.Clamp(HP - amount, 0, 100);
            HPUpdate?.Invoke(HP);
            if (HP == 0)
                Destroy?.Invoke();
        }

        /// <summary>
        /// Restores HP by <paramref name="amount"/> percent.
        /// Clamps to 100. Does nothing if already dead.
        /// </summary>
        public void Heal(int amount)
        {
            if (IsDestroy) return;
            HP = Mathf.Clamp(HP + amount, 0, 100);
            HPUpdate?.Invoke(HP);
        }


    }
}
