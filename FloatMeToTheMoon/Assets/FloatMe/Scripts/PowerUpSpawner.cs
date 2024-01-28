using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> oxygenPowerUps = new List<GameObject>();
        [SerializeField] private List<GameObject> rewindPowerUps = new List<GameObject>();
        // Add similar lists for other power-ups

        [SerializeField] GameObject oxygenPrefab, rewindPrefab, shieldPrefab, slownessPrefab, speedBoostPrefab, CoinAttractorPrefab;
        [SerializeField] Transform spawner;
        [SerializeField] private float spawnIntervalOxygen, spawnIntervalRewind; // Add similar variables for other power-ups
        [SerializeField] private float maxOxygenPowerUps, maxRewindPowerUps, maxShieldPowerUps, maxSlownessPowerUps, maxSpeedBoostPowerUps, maxCoinAttractorPowerUps;

        private void Update()
        {
            SpawnPowerUp(ref spawnIntervalOxygen, oxygenPowerUps, oxygenPrefab, maxOxygenPowerUps);
            SpawnPowerUp(ref spawnIntervalRewind, rewindPowerUps, rewindPrefab, maxRewindPowerUps);
            // Add similar calls for other power-ups
        }

        private void SpawnPowerUp(ref float spawnInterval, List<GameObject> powerUpList, GameObject powerUpPrefab, float maxPowerUps)
        {
            spawnInterval -= Time.deltaTime;

            if (spawnInterval <= 0 && powerUpList.Count < maxPowerUps)
            {
                CreatePowerUp(powerUpList, powerUpPrefab);
                ResetTimer(ref spawnInterval);
            }
            else if (spawnInterval <= 0)
            {
                SpawnExistingPowerUp(powerUpList);
                ResetTimer(ref spawnInterval);
            }
        }

        private void SpawnExistingPowerUp(List<GameObject> powerUpList)
        {
            float randomX = Random.Range(-1.15f, 1.15f);

            GameObject powerUp = powerUpList.Find(p => !p.activeSelf);

            if (powerUp == null)
            {
                return;
            }

            powerUp.SetActive(true);
            powerUp.transform.position = new Vector2(randomX, spawner.position.y);
        }

        private void ResetTimer(ref float spawnInterval)
        {
            spawnInterval = Random.Range(1, 6);
        }

        private GameObject CreatePowerUp(List<GameObject> powerUpList, GameObject powerUpPrefab)
        {
            float randomX = Random.Range(-1.15f, 1.15f);

            GameObject powerUp = Instantiate(powerUpPrefab, new Vector2(randomX, spawner.position.y), Quaternion.identity);
            powerUpList.Add(powerUp);
            return powerUp;
        }
    }
}
