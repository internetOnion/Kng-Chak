using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KngChak.Flow
{
    /// <summary>
    /// Runs the game loop: Start → Playing → Game over → (tap) → Playing again.
    /// Time is frozen outside Playing. A tap or click on the start or game over screen begins a run
    /// and raises GameEvents.RunStarted, so every system can reset itself for wave 1.
    /// Put one in the scene.
    /// </summary>
    public class GameFlow : MonoBehaviour
    {
        public enum State
        {
            Start,
            Playing,
            GameOver,
        }

        /// <summary>The state the game is in now.</summary>
        public State Current { get; private set; } = State.Start;

        /// <summary>
        /// Raised whenever the state changes.
        /// Parameter: the new state.
        /// Consumers: start and game over screens.
        /// </summary>
        public event Action<State> StateChanged;

        void OnEnable()
        {
            GameEvents.OnGameOver += HandleGameOver;
        }

        void OnDisable()
        {
            GameEvents.OnGameOver -= HandleGameOver;
        }

        void Start()
        {
            Enter(State.Start);
        }

        void Update()
        {
            if (Current == State.Playing) return;

            // Pointer covers both touch and mouse, so the same code works on phone and in the editor
            Pointer pointer = Pointer.current;
            if (pointer != null && pointer.press.wasPressedThisFrame)
            {
                BeginRun();
            }
        }

        void OnDestroy()
        {
            // Don't leave time frozen when the scene unloads or Play mode stops
            Time.timeScale = 1f;
        }

        private void BeginRun()
        {
            Enter(State.Playing);
            GameEvents.RunStarted();
        }

        private void HandleGameOver()
        {
            if (Current == State.Playing) Enter(State.GameOver);
        }

        private void Enter(State next)
        {
            Current = next;
            Time.timeScale = next == State.Playing ? 1f : 0f;
            StateChanged?.Invoke(next);
        }
    }
}
