using FloatMeToTheMoon.Player;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class AirController : MonoBehaviour
    {
        [SerializeField] private float airTotal;
        [SerializeField] private float air;
        [SerializeField] private bool dead;
        PlayerMovement playerMovement;
        Animator animator;
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            air = airTotal;

        }
        private void Update()
        {
            air -= Time.deltaTime * playerMovement.Speed;
            animator.SetFloat("Air", air);

            //70% de AireTotal(100) es 70 y el 30% 30  

            if (air >= airTotal * 70 / 100 && dead) ///esta lleno
            {

            }
            else if (air < airTotal * 70 / 100 && dead) ///esta a la mitad
            {

            }
            else if (air < airTotal * 30 / 100 && dead) /// esta en las ultimas
            {

            }else if(air <= 0) ///Se desinflo
            {
                
            }
        }


    }
}
