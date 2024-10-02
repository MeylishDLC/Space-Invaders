using FMODUnity;
using UnityEngine;

namespace Sound
{
    public class FMODEvents: MonoBehaviour
    {
        [field: Header("UI SFX")]
        [field: SerializeField] public EventReference enemyDeathSound { get; private set; }
        [field: SerializeField] public EventReference hitPlayerSound { get; private set; }
        [field: SerializeField] public EventReference playerDeathSound { get; private set; }
        [field: SerializeField] public EventReference shootSound { get; private set; }
        [field: SerializeField] public EventReference enemyWalkSound { get; private set; }
    }
}