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


        [Header("********************* SpeedBoost PowerUP *********************")]
        [Space(10)]
        [SerializeField] float maxSpeed;
        [SerializeField] float speedBoostTime;

        [Header("********************* SpeedReduction PowerUP *****************")]
        [Space(10)]
        [SerializeField] float minSpeed;
        [SerializeField] float speedReductionTime;

        [Header("********************* Rewind PowerUP *************************")]
        [Space(10)]
        [SerializeField] int rewindWaitTime;
        [SerializeField] bool canRewind;
        [SerializeField] bool playerHit;
        [SerializeField] List<Vector2> positions = new List<Vector2>();
        Coroutine rewindCoroutine;

        [Header("******************** Shield PowerUp **************************")]
        [Space(10)]
        [SerializeField] private bool isShieldActive;
        [SerializeField] private GameObject shield;
        [SerializeField] private AnimationClip shieldEndAnimation;
        private Animator shieldAnimator;
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            //shieldAnimator = shield.GetComponent<Animator>();
        }

        private void Start()
        {
            baseSpeed = playerMovement.Speed;
            rewindCoroutine = StartCoroutine(RewindCoroutine());
        }
        private void FixedUpdate()
        {
            Rewind();
        }
        private void Rewind()
        {
            if (canRewind && !playerHit)
            {
                positions.Insert(0, transform.position);
            }
            if (positions.Count > 0 && playerHit && canRewind)
            {
                StopCoroutine(rewindCoroutine);
                transform.position = positions[0];
                positions.RemoveAt(0);
                playerMovement.Speed = 0;

                if (positions.Count == 0)
                {
                    playerMovement.Speed = baseSpeed;

                    canRewind = false;
                    playerHit = false;
                    positions.Clear();
                }
            }
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
            yield return new WaitForSeconds(rewindWaitTime);
            canRewind = false;
            positions.Clear();
        }

        IEnumerator CoinCollectionCoroutine()
        {
            yield return new WaitForSeconds(speedBoostTime);
        }

        IEnumerator ShieldCoroutine()
        {

            yield return new WaitForSeconds(shieldEndAnimation.length);
            isShieldActive = false;
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
            else if (other.CompareTag("Rewind"))
            {
                StartCoroutine(RewindCoroutine());
                canRewind = true;
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("OxygenRefill"))
            {

            }
            else if (other.CompareTag("CoinCollection"))
            {
                StartCoroutine(CoinCollectionCoroutine());
            }
            else if (other.CompareTag("Shield"))
            {
                isShieldActive = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                if (isShieldActive)
                {
                    StartCoroutine(ShieldCoroutine());
                }
                else
                {
                    playerHit = true;
                }
            }
        }
    }
}
