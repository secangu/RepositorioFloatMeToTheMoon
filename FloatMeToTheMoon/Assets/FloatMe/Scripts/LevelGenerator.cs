using UnityEngine;

namespace FloatMeToTheMoon
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] levelParts;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float minDistance;
        [SerializeField] private GameObject finalPart; // Nueva variable para la parte final
        [SerializeField] private bool infinity; // Nueva variable para la parte final

        private int currentIndex;
        private Transform player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
            currentIndex = 0;


            for (int i = 0; i < levelParts.Length; i++)
            {
                GenerateLevelParts();
            }
                GenerateFinalPart(); // Después de generar las partes iniciales, agrega la parte final

        }

        private void Update()
        {
            //Para hacer el nivel infinito
            if (infinity && Vector2.Distance(player.position, endPoint.position) < minDistance)
            {
                GenerateFinalPart();
            }
        }

        private void GenerateLevelParts()
        {
            GameObject level = Instantiate(levelParts[currentIndex], endPoint.position, Quaternion.identity);
            endPoint = FindEndPoint(level);

            currentIndex++; // Avanzamos al siguiente índice en la lista
        }
        private void GenerateFinalPart()
        {
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
