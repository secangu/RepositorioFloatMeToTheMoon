using UnityEngine;

namespace FloatMeToTheMoon.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float sensitivityAdjustX;

        public float Speed { get => speed; set => speed = value; }

        private void Awake()
        {
            // Inicialización, si es necesario
        }

        private void Start()
        {
            // Inicialización, si es necesario
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            // Move constantemente hacia arriba
            transform.Translate(Vector2.up * Speed * Time.deltaTime);

            if (Input.touchCount > 0)
            {
                // Obtener el primer toque
                Touch touch = Input.GetTouch(0);

                // Verificar si el toque se desplaza horizontalmente
                if (touch.phase == TouchPhase.Moved)
                {
                    // Obtener el cambio de posición en el eje X
                    float adjustHorizontal = touch.deltaPosition.x * sensitivityAdjustX;

                    // Calcular nueva posición
                    Vector3 newPosition = transform.position + Vector3.right * adjustHorizontal * Time.deltaTime;

                    // Limitar la posición en el eje X para evitar que el objeto salga de la pantalla
                    float limitX = Mathf.Clamp(newPosition.x, -1.5f, 1.5f);
                    transform.position = new Vector3(limitX, newPosition.y, newPosition.z);
                }
            }
        }
    }
}
