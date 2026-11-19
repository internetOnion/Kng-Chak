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
        /// Parameter: the wave number, starting at 1.
        /// Producers: WaveManager
        /// Consumers: HUD (wave number), Audio
        /// </summary>
        public static event Action<int> OnWaveStarted;
        public static void WaveStarted(int waveNumber) => OnWaveStarted?.Invoke(waveNumber);

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

        /// <summary>
        /// Fired when a run begins: the first tap on the start screen, or a tap to restart after game over.
        /// Every system resets itself here, ready for wave 1.
        /// Producers: GameFlow
        /// Consumers: PlanetHealth (full HP), WaveManager (start wave 1), Ship, Rocks
        /// </summary>
        public static event Action OnRunStarted;
        public static void RunStarted() => OnRunStarted?.Invoke();

        /// <summary>
        /// Fired whenever planet HP changes: damage, heal or reset.
        /// Parameter: the new HP, in percent (0–100).
        /// Producers: PlanetHealth
        /// Consumers: HUD
        /// </summary>
        public static event Action<int> OnPlanetHPChanged;
        public static void PlanetHPChanged(int hp) => OnPlanetHPChanged?.Invoke(hp);

        /// <summary>
        /// Fired whenever the run's score changes.
        /// Parameter: the new total score.
        /// Producers: whoever awards score (combat, combos)
        /// Consumers: HUD
        /// </summary>
        public static event Action<int> OnScoreChanged;
        public static void ScoreChanged(int total) => OnScoreChanged?.Invoke(total);
    }
}
