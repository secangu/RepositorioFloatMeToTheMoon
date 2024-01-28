using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

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
        [SerializeField] private TextMeshProUGUI score_Text;
        [SerializeField] private TextMeshProUGUI highScore_Text;
        [SerializeField] private TextMeshProUGUI coinsCollected_Text;
        private string encryptionKey = "FMTTMKGSGGJCOL01";
        
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

            // Incrementa el puntaje basándose en la distancia recorrida y la tasa de puntuación
            currentScore += distanceTraveled * scoringRate;

            // Actualiza la última posición conocida del jugador
            lastPlayerPosition = player.position;

            // Actualiza el puntaje máximo si el puntaje actual supera al puntaje máximo
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
                HighScore = highScore  // Agrega el puntaje máximo al objeto de datos
            };

            // Convierte el objeto a formato JSON
            string jsonData = JsonUtility.ToJson(data);

            // Cifra el JSON utilizando AES
            string encryptedData = Encrypt(jsonData, encryptionKey);

            // Guarda el JSON cifrado en un archivo en la carpeta persistente de datos
            string filePath = Path.Combine(Application.persistentDataPath, "GameData.dat"); // Cambia la extensión del archivo
            File.WriteAllText(filePath, encryptedData);
            Debug.Log("Ruta a la carpeta persistente de datos: " + Application.persistentDataPath);
        }

        private void LoadGameData()
        {
            // Carga el JSON cifrado desde el archivo en la carpeta persistente de datos
            string filePath = Path.Combine(Application.persistentDataPath, "GameData.dat"); // Cambia la extensión del archivo

            if (File.Exists(filePath))
            {
                string encryptedData = File.ReadAllText(filePath);

                // Descifra el JSON utilizando AES
                string jsonData = Decrypt(encryptedData, encryptionKey);

                // Convierte el JSON a un objeto
                GameData data = JsonUtility.FromJson<GameData>(jsonData);

                // Asigna los valores cargados a las variables
                coinsCollected += data.CoinsCollected;
                highScore = data.HighScore;  // Asigna el puntaje máximo cargado
            }
        }

        private string Encrypt(string data, string key)
        {
            // Convierte la cadena a bytes
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            // Cifra los bytes utilizando AES
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.CFB;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(dataBytes, 0, dataBytes.Length);
                    }

                    return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
                }
            }
        }

        private string Decrypt(string data, string key)
        {
            // Convierte la cadena cifrada a bytes
            byte[] cipherText = Convert.FromBase64String(data);
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            // Descifra los bytes utilizando AES
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.CFB;

                byte[] iv = cipherText.Take(aesAlg.BlockSize / 8).ToArray();
                byte[] encryptedData = cipherText.Skip(aesAlg.BlockSize / 8).ToArray();

                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class GameData
    {
        public int CoinsCollected;
        public float HighScore;  // Agrega el campo para almacenar el puntaje máximo
    }
}
