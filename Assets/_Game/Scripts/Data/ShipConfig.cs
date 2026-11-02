using UnityEngine;

namespace KngChak.Data
{
    [CreateAssetMenu(fileName = "ShipConfig", menuName = "Kng Chak/Ship Config")]
    public class ShipConfig : ScriptableObject
    {
        [Header("Speed")]
        [field: SerializeField] public float MaxSpeed { get; private set; } = 22f;
        [field: SerializeField] public float Drag { get; private set; } = 0.45f;
        [field: SerializeField] public float ThrustAccel { get; private set; } = 32f; // speed gained per second while thrusting
        [field: SerializeField] public float Brake { get; private set; } = 2.6f; // like Drag, but while right click is held

        [Header("Steering")]
        [field: SerializeField] public float TurnRate { get; private set; } = 315f; // degrees per second
        [field: SerializeField] public float ThrustConeDeg { get; private set; } = 75f;
        [field: SerializeField] public float Grip { get; private set; } = 1.7f;

        [Header("Cursor")]
        [field: SerializeField] public float CursorDeadzone { get; private set; } = 2.5f;
        [field: SerializeField] public float CursorFullRange { get; private set; } = 12f;

        [Header("Boost")]
        [field: SerializeField] public float BoostSpeed { get; private set; } = 46f;
        [field: SerializeField] public float BoostDuration { get; private set; } = 0.45f;

        [Header("Fuel")]
        [field: SerializeField] public float FuelMax { get; private set; } = 100f;
        [field: SerializeField] public float BoostCost { get; private set; } = 30f;   // one "segment": the fuel one boost burst uses
        [field: SerializeField] public float FuelRegen { get; private set; } = 7f;    // fuel per second, refilled while not boosting
    }
}
