using KngChak.Flow;
using TMPro;
using UnityEngine;

namespace KngChak.UI
{
    /// <summary>
    /// In-game HUD: planet HP, wave number and score.
    /// Reads everything from GameEvents, so it never talks to other systems directly.
    /// Put it on the HUD canvas and drag in the three texts.
    /// </summary>
    public class Hud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _waveText;
        [SerializeField] private TMP_Text _scoreText;

        void OnEnable()
        {
            GameEvents.OnPlanetHPChanged += ShowHP;
            GameEvents.OnWaveStarted += ShowWave;
            GameEvents.OnScoreChanged += ShowScore;
            GameEvents.OnRunStarted += HandleRunStarted;
        }

        void OnDisable()
        {
            GameEvents.OnPlanetHPChanged -= ShowHP;
            GameEvents.OnWaveStarted -= ShowWave;
            GameEvents.OnScoreChanged -= ShowScore;
            GameEvents.OnRunStarted -= HandleRunStarted;
        }

        private void ShowHP(int hp)
        {
            if (_hpText != null) _hpText.text = $"HP {hp}%";
        }

        private void ShowWave(int waveNumber)
        {
            if (_waveText != null) _waveText.text = $"WAVE {waveNumber}";
        }

        private void ShowScore(int total)
        {
            if (_scoreText != null) _scoreText.text = $"SCORE {total}";
        }

        // A new run starts from zero. HP and wave come back through their own events.
        private void HandleRunStarted()
        {
            ShowScore(0);
        }
    }
}
