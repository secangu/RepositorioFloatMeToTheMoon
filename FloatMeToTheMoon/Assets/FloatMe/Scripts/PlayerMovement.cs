using UnityEngine;
using UnityEngine.UI;

namespace FloatMeToTheMoon.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Slider sensitivitySlider;
        private float sensitivityAdjustX;
        [SerializeField] private bool isMoving;

        Animator animator;

        public float Speed { get => speed; set => speed = value; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            // Configurar el valor inicial del slider y la sensibilidad
            sensitivitySlider.minValue = 0.01f;
            sensitivitySlider.maxValue = 0.6f;

            // Si PlayerPrefs contiene la sensibilidad almacenada, úsala; de lo contrario, usa el valor predeterminado
            if (PlayerPrefs.HasKey("SensitivityAdjustX"))
            {
                sensitivityAdjustX = PlayerPrefs.GetFloat("SensitivityAdjustX");
            }
            else
            {
                sensitivityAdjustX = 0.2f; // Valor predeterminado
                PlayerPrefs.SetFloat("SensitivityAdjustX", sensitivityAdjustX);
            }

            sensitivitySlider.value = sensitivityAdjustX;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityValueChanged);
        }

        private void FixedUpdate()
        {
            if (IsMobilePlatform())
            {
                MobileMovement();
            }
            else
            {
                PCMovement();
            }
        }

        private void PCMovement()
        {
            // Mover constantemente hacia arriba
            transform.Translate(Vector2.up * Speed * Time.deltaTime);

            if (Input.GetMouseButton(0))
            {
                float adjustHorizontal = Input.GetAxis("Mouse X") * sensitivityAdjustX * 6;
                Vector3 newPosition = transform.position + Vector3.right * adjustHorizontal * Time.deltaTime;
                float limitX = Mathf.Clamp(newPosition.x, -1.15f, 1.15f);
                transform.position = new Vector3(limitX, newPosition.y, newPosition.z);
                isMoving = true;

                if (adjustHorizontal < 0)
                {
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y);
                }
                else if (adjustHorizontal > 0)
                {
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
            }
            else
            {
                isMoving = false;
            }

            animator.SetBool("Move", isMoving);
        }

        private void MobileMovement()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    float adjustHorizontal = touch.deltaPosition.x * sensitivityAdjustX;
                    Vector3 newPosition = transform.position + Vector3.right * adjustHorizontal * Time.deltaTime;
                    float limitX = Mathf.Clamp(newPosition.x, -1.15f, 1.15f);
                    transform.position = new Vector3(limitX, newPosition.y, newPosition.z);
                    isMoving = true;

                    if (adjustHorizontal < 0)
                    {
                        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y);
                    }
                    else if (adjustHorizontal > 0)
                    {
                        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                    }
                }
            }
            else
            {
                isMoving = false;
            }

            animator.SetBool("Move", isMoving);
        }

        private void OnSensitivityValueChanged(float value)
        {
            sensitivityAdjustX = value;
            PlayerPrefs.SetFloat("SensitivityAdjustX", sensitivityAdjustX);
        }

        private bool IsMobilePlatform()
        {
            return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }
}
