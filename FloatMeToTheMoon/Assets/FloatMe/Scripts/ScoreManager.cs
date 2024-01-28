using UnityEngine;
using System.IO;

namespace FloatMeToTheMoon
{
    public class ScoreManager : MonoBehaviour
    {
        private Transform player;
        private float scoringRate = 1f;
        [SerializeField] private float currentScore;
        [SerializeField] private float highScore;
        [SerializeField] private int coinsCollected;
        private Vector3 lastPlayerPosition;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
            InitializeGameData();
            lastPlayerPosition = player.position;
        }

        private void Update()
        {
            CalculateScore();
        }

        void CalculateScore()
        {
            float distanceTraveled = Vector3.Distance(lastPlayerPosition, player.position);
            currentScore += distanceTraveled * scoringRate;
            lastPlayerPosition = player.position;

            if (currentScore > highScore)
            {
                highScore = currentScore;
                SaveGameData();
            }
        }

        public void CollectCoin()
        {
            coinsCollected++;
            SaveGameData();
        }

        private void InitializeGameData()
        {
            LoadGameData();
        }

        private void SaveGameData()
        {
            // Crea un objeto de datos que quieres almacenar
            GameData data = new GameData
            {
                CoinsCollected = coinsCollected,
                PlayerScore = currentScore,
                HighScore = highScore
            };

            // Convierte el objeto a formato JSON
            string jsonData = JsonUtility.ToJson(data);

            // Cifra los datos antes de guardarlos
            string encryptedData = EncryptionManager.Encrypt(jsonData);

            // Guarda el JSON cifrado en un archivo en la carpeta persistente de datos
            string filePath = Path.Combine(Application.persistentDataPath, "Data.dat");
            File.WriteAllText(filePath, encryptedData);
        }

        private void LoadGameData()
        {
            // Carga el JSON cifrado desde el archivo en la carpeta persistente de datos
            string filePath = Path.Combine(Application.persistentDataPath, "Data.dat");

            if (File.Exists(filePath))
            {
                string encryptedData = File.ReadAllText(filePath);

                // Descifra los datos antes de convertirlos a un objeto
                string jsonData = EncryptionManager.Decrypt(encryptedData);

                // Convierte el JSON a un objeto
                GameData data = JsonUtility.FromJson<GameData>(jsonData);

                // Asigna los valores cargados a las variables
                coinsCollected = data.CoinsCollected;
                currentScore = data.PlayerScore;
                highScore = data.HighScore;
            }
        }
    }

    [System.Serializable]
    public class GameData
    {
        public int CoinsCollected;
        public float PlayerScore;
        public float HighScore;
    }
}
