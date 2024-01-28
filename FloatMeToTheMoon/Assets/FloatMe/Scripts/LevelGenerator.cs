using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FloatMeToTheMoon
{
    public class LevelGenerator : MonoBehaviour
    {
        public enum LevelPartType
        {
            Initial,
            Part2,
            Part3,
            Part4,
            Part5,
            Part6,
            Part7,
            Part8,
            Part9,
            Part10,
            Final
        }

        [System.Serializable]
        public struct LevelPartPrefab
        {
            public LevelPartType partType;
            public GameObject[] prefabs;
        }

        [SerializeField] private List<LevelPartType> levelOrder;
        [SerializeField] private List<LevelPartPrefab> levelParts;
        [SerializeField] private Transform initialEndPoint;
        [SerializeField] private float minDistance;
        [SerializeField] private bool isInfiniteMode;

        private Transform player;
        private Transform endPoint;
        private GameController gameController;        
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            gameController = FindObjectOfType<GameController>();
            if (player == null)
            {
                Debug.LogError("Player null");
            }
        }

        private void Start()
        {
            isInfiniteMode = gameController.IsInfiniteMode;
            endPoint = initialEndPoint;

            foreach (LevelPartType partType in levelOrder)
            {
                if (isInfiniteMode && partType == LevelPartType.Final)
                {
                    // Evitar la generación de la parte final si infinity está activo
                    break;
                }
                GenerateLevelPart(partType);

            }
        }

        private void Update()
        {
            if (isInfiniteMode && Vector2.Distance(player.position, endPoint.position) < minDistance)
            {
                // Genera aleatoriamente entre la parte 9 y 10
                LevelPartType randomPartType = Random.Range(0, 2) == 0 ? LevelPartType.Part9 : LevelPartType.Part10;
                GenerateLevelPart(randomPartType);
            }

        }

        private void GenerateLevelPart(LevelPartType partType)
        {
            LevelPartPrefab partPrefab = levelParts.Find(x => x.partType == partType);

            if (partPrefab.prefabs != null && partPrefab.prefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, partPrefab.prefabs.Length);
                GameObject levelPart = Instantiate(partPrefab.prefabs[randomIndex], endPoint.position, Quaternion.identity);
                endPoint = FindEndPoint(levelPart);
            }
            else
            {
                Debug.LogError("Prefab en array no esta asignado para LevelPartType: " + partType);
            }
        }

        private Transform FindEndPoint(GameObject levelPart)
        {
            Transform point = null;

            foreach (Transform location in levelPart.transform)
            {
                if (location.CompareTag("EndPoint"))
                {
                    point = location;
                    break;
                }
            }

            return point;
        }
    }
}