using UnityEngine;
using UnityEngine.UI;

namespace FloatMeToTheMoon
{
    public class GameController : MonoBehaviour
    {
        [Tooltip("Objetos que se activarán al comenzar el juego.")]
        [SerializeField] private GameObject[] objectsToActivate;

        [Tooltip("Objetos que se desactivarán al comenzar el juego.")]
        [SerializeField] private GameObject[] objectsToDeactivate;

        [SerializeField] private Image infinityModeButton;
        [SerializeField] private Sprite infinityModeON;
        [SerializeField] private Sprite infinityModeOFF;
        [SerializeField] private bool isInfiniteMode;

        public bool IsInfiniteMode { get => isInfiniteMode; set => isInfiniteMode = value; }

        private void Start()
        {
            ToggleInfinityMode(isInfiniteMode);
            // Desactivar objetos que están activados 
            SetObjectsActive(objectsToDeactivate, true);

            // Activar objetos que están desactivados 
            SetObjectsActive(objectsToActivate, false);
        }

        public void ActivateObjectsFromButton()
        {
            SetObjectsActive(objectsToDeactivate, false);
            SetObjectsActive(objectsToActivate, true);
        }

        private void SetObjectsActive(GameObject[] objects, bool isActive)
        {
            foreach (GameObject obj in objects)
            {
                if (obj != null)
                {
                    obj.SetActive(isActive);
                }
            }
        }
        private void ToggleInfinityMode(bool enable)
        {
            isInfiniteMode = enable;
            infinityModeButton.sprite = enable ? infinityModeON : infinityModeOFF;
        }

        public void ToggleInfinityMode()
        {
            // Cambia entre infinito y no infinito
            ToggleInfinityMode(!isInfiniteMode);
        }
    }
}
