using UnityEngine;

namespace KngChak

{
    /// <summary>                                                                                                                                                                                   
    /// Debug-only listener that logs all GameEvents to the console.                                                                                                                                
    /// Attach to any GameObject in the test scene to verify the hub works.                                                                                                                         
    /// Remove (or disable) before shipping.                                                                                                                                                        
    /// </summary> 
    public class GameEventLogger : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void OnEnable()
        {
            GameEvents.OnRockBroken += LogRockBroken;
            GameEvents.OnRockPrimed += LogRockPrimed;
            GameEvents.OnPlanetHit += LogPlanetHit;
            GameEvents.OnWaveStarted += LogWaveStarted;
            GameEvents.OnWaveCleared += LogWaveCleared;
            GameEvents.OnGameOver += LogGameOver;
        }

        void OnDisable()
        {
            GameEvents.OnRockBroken -= LogRockBroken;
            GameEvents.OnRockPrimed -= LogRockPrimed;
            GameEvents.OnPlanetHit -= LogPlanetHit;
            GameEvents.OnWaveStarted -= LogWaveStarted;
            GameEvents.OnWaveCleared -= LogWaveCleared;
            GameEvents.OnGameOver -= LogGameOver;
        }

        void Start()
        {

        }

        void LogRockBroken() => Debug.Log("[GameEvents] RockBroken");
        void LogRockPrimed() => Debug.Log("[GameEvents] RockPrimed");
        void LogPlanetHit() => Debug.Log("[GameEvents] PlanetHit");
        void LogWaveStarted() => Debug.Log("[GameEvents] WaveStarted");
        void LogWaveCleared() => Debug.Log("[GameEvents] WaveCleared");
        void LogGameOver() => Debug.Log("[GameEvents] GameOver");

        // Update is called once per frame

    }
}
