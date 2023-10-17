using System;
using Enemies;
using TMPro;
using UnityEngine;

namespace DDA
{
    public class DDA : MonoBehaviour
    {
        public static Difficulties CurrentDifficulty;
        public static event Action<Difficulties> EmitDifficultyUpdate;
        
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private float intervalCheck;
        
        private float _currentIntervalCheck;

        private void Start()
        {
            CurrentDifficulty = Difficulties.Medium;
            _currentIntervalCheck = intervalCheck;
        }

        private void Update()
        {
            _currentIntervalCheck -= Time.deltaTime;
            if (_currentIntervalCheck <= 0) UpdateDifficulty();
        }

        private void UpdateDifficulty()
        {
            switch (playerHealth.GetHealthPoints)
            {
                case 1:
                    CurrentDifficulty = Difficulties.Easy;
                    break;
                case 2:
                    CurrentDifficulty = Difficulties.Medium;
                    break;
                case 3:
                    CurrentDifficulty = Difficulties.Hard;
                    break;
                default:
                    Debug.LogWarning("Default case hit DDA.UpdateDifficulty");
                    break;
            }

            _currentIntervalCheck = intervalCheck;
            EmitDifficultyUpdate?.Invoke(CurrentDifficulty);
        }
    }
}