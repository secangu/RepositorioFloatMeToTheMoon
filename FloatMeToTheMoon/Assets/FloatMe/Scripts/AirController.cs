using FloatMeToTheMoon.Player;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class AirController : MonoBehaviour
    {
        [SerializeField] private float airTotal, air70, air30;
        [SerializeField] private float air;
        private PlayerMovement playerMovement;
        private Animator animator;
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            air = airTotal;
            air70 = airTotal * 70 / 100;
            air30 = airTotal * 30 / 100;
        }
        private void Update()
        {
            float previousAir = air;
            air -= Time.deltaTime * playerMovement.Speed;
            animator.SetFloat("Air", air);

            // Verificar si el valor de air ha cruzado de positivo a negativo
            if (previousAir > 0 && air <= 0)
            {
                PlayerDied();
            }


            //70% de AireTotal(100) es 70 y el 30% 30            
        }
        public void PlayerDied()
        {
            playerMovement.enabled = false;
            this.GetComponent<Collider2D>().enabled = false;

            if (air >= air70) ///esta lleno
            {
                animator.Play("100%Dead");
            }
            else if (air >= air30) ///esta a la mitad
            {
                animator.Play("70%Dead");
            }
            else  /// esta en las ultimas
            {
                animator.Play("30%Dead");
            }
        }
        public void IncreaseAir(float addAir)
        {
            air += addAir;

            if (air >= airTotal)
            {
                air = airTotal;
            }
        }
    }
}
