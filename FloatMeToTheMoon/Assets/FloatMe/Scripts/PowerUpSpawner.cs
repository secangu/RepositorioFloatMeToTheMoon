using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] powerUpPrefabs;
        [SerializeField] private float spawnInterval = 5f;
        [SerializeField] private Vector2 spawnPoint;

        private void Start()
        {
            // Comienza el proceso de spawn de power-ups
            InvokeRepeating("SpawnPowerUp", 0f, spawnInterval);
        }

        private void SpawnPowerUp()
        {
            // Selecciona un power-up aleatorio del array de prefabs
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject powerUpPrefab = powerUpPrefabs[randomIndex];

            // Instancia el power-up en el punto de spawn 
            Instantiate(powerUpPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
