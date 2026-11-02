using UnityEngine;

namespace KngChak.Data
{
    [CreateAssetMenu(menuName = "Kng Chak/Arena Config")]
    public class ArenaConfig : ScriptableObject
    {
        [Header("Planet")]
        [field: SerializeField] public float PlanetRadius { get; private set; } = 10f;
        [field: SerializeField] public float OrbitBandTop { get; private set; } = 4.5f;

        [Header("Arena")]
        [field: SerializeField] public float ArenaRadius { get; private set; } = 66f;
        [field: SerializeField] public float EdgeShowDist { get; private set; } = 8f;

        [Header("Camera")]
        [field: SerializeField] public float CamView { get; private set; } = 50f;
        [field: SerializeField] public float CamNear { get; private set; } = 15f; // ship closer than this to the planet's centre: camera stays on the planet
        [field: SerializeField] public float CamFar { get; private set; } = 30f; // ship farther than this: camera centres on the ship
        [field: SerializeField] public float CamPlanetShow { get; private set; } = 0.35f; // at least this fraction of the planet's width always stays on screen

        public float OrbitRadius => PlanetRadius + OrbitBandTop;

        // Runs when a value is changed in the Inspector: keep values that only make sense together valid.
        private void OnValidate()
        {
            CamFar = Mathf.Max(CamFar, CamNear + 0.01f);   // the camera blend needs Near < Far
            CamPlanetShow = Mathf.Clamp01(CamPlanetShow);   // a fraction: 0 to 1
            EdgeShowDist = Mathf.Max(EdgeShowDist, 0.01f);  // the edge fade divides by this
        }
    }
}