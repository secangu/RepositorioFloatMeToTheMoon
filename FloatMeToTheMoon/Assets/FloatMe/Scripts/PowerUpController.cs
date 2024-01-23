using FloatMeToTheMoon.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class PowerUpController : MonoBehaviour
    {
        PlayerMovement playerMovement;
        private float baseSpeed;


        [Header("SpeedBoost PowerUP")]
        [SerializeField] float maxSpeed;
        [SerializeField] float speedBoostTime;

        [Header("SpeedReduction PowerUP")]
        [SerializeField] float minSpeed;
        [SerializeField] float speedReductionTime;

        [Header("Rewind PowerUP")]
        [SerializeField] int rewindWaitTime;
        [SerializeField] bool canRewind;
        [SerializeField] bool playerHit;
        [SerializeField] List<Vector2> positions = new List<Vector2>();

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            baseSpeed = playerMovement.Speed;
        }
        private void Update()
        {
            Rewind();
        }


        IEnumerator SpeedBoostCoroutine()
        {
            playerMovement.Speed = maxSpeed;

            yield return new WaitForSeconds(speedBoostTime);

            playerMovement.Speed = baseSpeed;
        }

        IEnumerator SpeedReductionCoroutine()
        {
            playerMovement.Speed = minSpeed;

            yield return new WaitForSeconds(speedReductionTime);

            playerMovement.Speed = baseSpeed;
        }

        IEnumerator RewindCoroutine()
        {

            yield return new WaitForSeconds(5);
            canRewind = false;
        }

        IEnumerator CoinCollectionCoroutine()
        {
            yield return new WaitForSeconds(speedBoostTime);
        }

        private void Rewind()
        {
            if (canRewind && !playerHit)
            {
                positions.Insert(0, transform.position);
            }
            if (positions.Count > 0 && playerHit && canRewind)
            {
                StopCoroutine(RewindCoroutine());
                transform.position = positions[0];
                positions.RemoveAt(0);
                playerMovement.Speed = 0;

                if (positions.Count == 0)
                {
                    canRewind = false;
                    playerHit = false;
                    playerMovement.Speed = baseSpeed;
                }
            }
        }



        private void OnTriggerEnter2D(Collider2D other)
        {
            /////////Power Ups
            if (other.CompareTag("SpeedBoost"))
            {
                Debug.Log("SpeedBoost");
                StartCoroutine(SpeedBoostCoroutine());
            }
            else if (other.CompareTag("SpeedReduction"))
            {
                StartCoroutine(SpeedReductionCoroutine());

            }
            else if (other.CompareTag("OxygenRefill"))
            {
                // Implementa la lógica para OxygenRefill
            }
            else if (other.CompareTag("Rewind"))
            {
                StartCoroutine(RewindCoroutine());
                canRewind = true;
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("CoinCollection"))
            {
                StartCoroutine(CoinCollectionCoroutine());
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                playerHit = true;
            }
        }
    }
}
