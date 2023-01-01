using System;
using Unity.Netcode;
using UnityEngine;

namespace ProjectWilson.Gameplay.GameplayObjects
{
    /// <summary>
    /// MonoBehaviour containing only one NetworkVariableInt which represents this object's health.
    /// </summary>
    public class NetworkHealthState : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<int> HP = new NetworkVariable<int>();
        public NetworkVariable<int> MaxHP = new NetworkVariable<int>();

        // public subscribable event to be invoked when HP has been fully depleted
        public event Action HPDepleted;

        // public subscribable event to be invoked when HP has been replenished
        public event Action HPReplenished;

        void OnEnable()
        {
            HP.OnValueChanged += OnChangeHP;
        }

        void OnDisable()
        {
            HP.OnValueChanged -= OnChangeHP;
        }

        void OnChangeHP(int previousValue, int newValue)
        {
            Debug.LogWarning($"[test] OnChangeHP : HP = {newValue}");
            if (previousValue > 0 && newValue <= 0)
            {
                // newly reached 0 HP
                HPDepleted?.Invoke();
            }
            else if (previousValue <= 0 && newValue > 0)
            {
                // newly revived
                HPReplenished?.Invoke();
            }
        }
    }
}
