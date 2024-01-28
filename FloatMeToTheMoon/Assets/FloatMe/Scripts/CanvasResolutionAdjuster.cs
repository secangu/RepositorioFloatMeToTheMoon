using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FloatMeToTheMoon
{
    public class CanvasResolutionAdjuster : MonoBehaviour
    {
        private Canvas canvas;

        [SerializeField] private Vector2 pcReferenceResolution = new Vector2(1920, 1080);
        [SerializeField] private Vector2 mobileReferenceResolution = new Vector2(800, 600);

        private void Start()
        {
            canvas = GetComponent<Canvas>();

            // Comprueba si la aplicación está en un dispositivo móvil
            if (Application.isMobilePlatform)
            {
                AdjustCanvasScaler(mobileReferenceResolution);
            }
            else
            {
                AdjustCanvasScaler(pcReferenceResolution);
            }
        }

        private void AdjustCanvasScaler(Vector2 referenceResolution)
        {
            CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
            if (canvasScaler != null)
            {
                canvasScaler.referenceResolution = referenceResolution;
            }
        }
    }
}
