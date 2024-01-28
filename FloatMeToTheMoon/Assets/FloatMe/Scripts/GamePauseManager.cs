using UnityEngine;
using UnityEngine.SceneManagement;

namespace FloatMeToTheMoon
{
    public class GamePauseManager : MonoBehaviour
    {
        [SerializeField] private AudioClip clickSound;
        public void PauseTime()
        {
            // Pausa el tiempo
            Time.timeScale = 0.0f;
        }
        public void ResumeTime()
        {
            // Reanuda el tiempo
            Time.timeScale = 1.0f;
        }
        public void RestartScene()
        {
            // Obtener el índice de la escena actual
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Cargar la escena actual nuevamente
            SceneManager.LoadScene(currentSceneIndex);
        }
        public void Click()
        {
            GetComponent<AudioSource>().PlayOneShot(clickSound);
        }
    }
}
