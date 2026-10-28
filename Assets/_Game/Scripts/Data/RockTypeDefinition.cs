using UnityEngine;

namespace KngChak.Data
{
    [CreateAssetMenu(menuName = "Kng Chak/Rock Type")]
    public class RockTypeDefinition : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public float Size { get; private set; }
        [field: SerializeField] public float FallSpeed { get; private set; }
        [field: SerializeField] public int DamagePercent { get; private set; }
        [field: SerializeField] public int Score { get; private set; }
        [field: SerializeField] public float PushMass { get; private set; }
        [field: SerializeField] public bool NeedsPrime { get; private set; }
        [field: SerializeField]
        public Color Tint { get; private set; } = Color.white;
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
    }
}