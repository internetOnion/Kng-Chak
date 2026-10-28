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

        public float OrbitRadius => PlanetRadius + OrbitBandTop;
    }
}