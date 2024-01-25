using FloatMeToTheMoon.Player;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class AirController : MonoBehaviour
    {
        [SerializeField] private float airTotal;
        [SerializeField] private float air;
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

        }


    }
}
