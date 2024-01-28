using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [Header("Power Up Lists")]
        [SerializeField] private List<PowerUpData> powerUpsData = new List<PowerUpData>();

        [Header("Spawner Settings")]
        [SerializeField] private Transform spawner;

        private void Start()
        {
            foreach (PowerUpData data in powerUpsData)
            {
                ResetTimer(ref data.interval, data);
            }
        }
        private void Update()
        {
            foreach (PowerUpData data in powerUpsData)
            {
                SpawnPowerUp(data);
            }
        }

        private void SpawnPowerUp(PowerUpData data)
        {
            data.interval -= Time.deltaTime;

            if (data.interval <= 0 && data.powerUpList.Count < data.maxPowerUps)
            {
                CreatePowerUp(data);
                ResetTimer(ref data.interval, data);
            }
            else if (data.interval <= 0)
            {
                SpawnExistingPowerUp(data.powerUpList);
                ResetTimer(ref data.interval, data);
            }
        }

        private void SpawnExistingPowerUp(List<GameObject> powerUpList)
        {
            float randomX = Random.Range(-1.15f, 1.15f);
            GameObject powerUp = powerUpList.Find(p => !p.activeSelf);

            if (powerUp != null)
            {
                powerUp.SetActive(true);
                powerUp.transform.position = new Vector2(randomX, spawner.position.y);
            }
        }

        private void ResetTimer(ref float spawnInterval, PowerUpData data)
        {
            spawnInterval = Random.Range(data.spawnInterval-5, data.spawnInterval);
        }

        private GameObject CreatePowerUp(PowerUpData data)
        {
            float randomX = Random.Range(-1.15f, 1.15f);
            GameObject powerUp = Instantiate(data.powerUpPrefab, new Vector2(randomX, spawner.position.y), Quaternion.identity);
            data.powerUpList.Add(powerUp);
            return powerUp;
        }

        [System.Serializable]
        private class PowerUpData
        {
            public List<GameObject> powerUpList;
            public GameObject powerUpPrefab;
            public float spawnInterval;
            public float interval;
            public float maxPowerUps;
        }
    }
}
