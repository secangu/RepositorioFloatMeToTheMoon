using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] levelParts;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float minDistance;
        [SerializeField] private int initialQuantity;
        [SerializeField] private GameObject finalPart; // Nueva variable para la parte final

        private Transform player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
            for (int i = 0; i < initialQuantity; i++)
            {
                GenerateLevelParts();
            }

            // Después de generar las partes iniciales, agrega la parte final
            GenerateFinalPart();
        }

        private void Update()
        {
            //Para hacer el nivel infinito
            //if (Vector2.Distance(player.position, endPoint.position) < minDistance)
            //{
            //    GenerateLevelParts();
            //}
        }

        private void GenerateLevelParts()
        {
            int random = Random.Range(0, levelParts.Length);
            GameObject level = Instantiate(levelParts[random], endPoint.position, Quaternion.identity);
            endPoint = FindEndPoint(level);
        }

        private void GenerateFinalPart()
        {
            // Asegúrate de que la parte final tenga un punto de conexión ("EndPoint")
            GameObject finalLevel = Instantiate(finalPart, endPoint.position, Quaternion.identity);
            endPoint = FindEndPoint(finalLevel);
        }

        private Transform FindEndPoint(GameObject levelPart)
        {
            Transform point = null;

            foreach (Transform ubication in levelPart.transform)
            {
                if (ubication.CompareTag("EndPoint"))
                {
                    point = ubication;
                    break;
                }
            }

            return point;
        }
    }
}
