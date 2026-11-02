using System;

namespace KngChak.Flow
{
    /// <summary>
    /// Central event hub for game-wide signals.
    /// Systems fire events here; UI, audio, and other systems subscribe here.
    /// No system should reach into another system's scripts directly.
    /// </summary>
    public static class GameEvents
    {
        /// <summary>
        /// Fired when a rock is fully destroyed.
        /// Producers: Rock
        /// Consumers: WaveManager, HUD, Audio
        /// </summary>
        public static event Action OnRockBroken;
        public static void RockBroken() => OnRockBroken?.Invoke();

        /// <summary>
        /// Fired when a rock enters its cracked / about-to-break state.
        /// Producers: Rock
        /// Consumers: Audio, VFX
        /// </summary>
        public static event Action OnRockPrimed;
        public static void RockPrimed() => OnRockPrimed?.Invoke();

        /// <summary>
        /// Fired when a rock collides with and damages the planet.
        /// Parameter: damage dealt, in HP percent.
        /// Producers: PlanetHealth
        /// Consumers: HUD (HP bar), Audio, Camera shake
        /// </summary>
        public static event Action<int> OnPlanetHit;
        public static void PlanetHit(int damage) => OnPlanetHit?.Invoke(damage);

        /// <summary>
        /// Fired at the beginning of each wave.
        /// Producers: WaveManager
        /// Consumers: HUD (wave number), Audio
        /// </summary>
        public static event Action OnWaveStarted;
        public static void WaveStarted() => OnWaveStarted?.Invoke();

        /// <summary>
        /// Fired when all rocks in a wave are destroyed.
        /// Producers: WaveManager
        /// Consumers: HUD, Audio, WaveManager (to start next wave)
        /// </summary>
        public static event Action OnWaveCleared;
        public static void WaveCleared() => OnWaveCleared?.Invoke();

        /// <summary>
        /// Fired when planet HP reaches zero. Game is lost.
        /// Producers: PlanetHealth
        /// Consumers: GameOverUI, Audio
        /// </summary>
        public static event Action OnGameOver;
        public static void GameOver() => OnGameOver?.Invoke();
    }
}
