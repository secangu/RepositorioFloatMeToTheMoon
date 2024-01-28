using FloatMeToTheMoon.Player;
using TMPro;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class AirController : MonoBehaviour
    {
        [SerializeField] private float airTotal, air70, air30;
        [SerializeField] private float air;
        [SerializeField] private GameObject deadScore;
        private PlayerMovement playerMovement;
        private Animator animator;
        [SerializeField] TextMeshProUGUI airPorcentText;
        public float Air { get => air; set => air = value; }

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            Air = airTotal;
            air70 = airTotal * 70 / 100;
            air30 = airTotal * 30 / 100;
        }
        private void Update()
        {
            float previousAir = Air;
            Air -= Time.deltaTime * playerMovement.Speed;
            animator.SetFloat("Air", Air);

            float airPorcent = ((Air / airTotal) * 100);
            airPorcentText.text = string.Format("{0:0.0}%", airPorcent);

            // Verificar si el valor de air ha cruzado de positivo a negativo
            if (previousAir > 0 && Air <= 0)
            {
                PlayerDied();
                Debug.Log("0 air");

            }


            //70% de AireTotal(100) es 70 y el 30% 30            
        }
        public void PlayerDied()
        {
            playerMovement.enabled = false;
            this.GetComponent<Collider2D>().enabled = false;

            if (Air >= air70) ///esta lleno
            {
                animator.Play("100%Dead");
            }
            else if (Air >= air30) ///esta a la mitad
            {
                animator.Play("70%Dead");
            }
            else  /// esta en las ultimas
            {
                animator.Play("30%Dead");
            }
            GetComponent<AirController>().enabled = false;
            Invoke("ActivateObject", 2f);

        }

        private void ActivateObject()
        {
            deadScore.SetActive(true);
        }
        public void IncreaseAir(float addAir)
        {
            Air += addAir;

            if (Air >= airTotal)
            {
                Air = airTotal;
            }
        }
    }
}
