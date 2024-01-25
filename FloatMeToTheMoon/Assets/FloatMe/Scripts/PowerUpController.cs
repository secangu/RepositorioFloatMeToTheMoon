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

        [Header("******************** CoinCollection PowerUp ******************")]
        [Space(10)]
        [SerializeField] private Transform coinCollection;
        [SerializeField] private Vector2 coinCollectionArea;
        [SerializeField] private float coinCollectionTime;
        [SerializeField] private float coinSpeed;
        [SerializeField] private bool isCoinCollectionActive;

        [Header("******************** Shield PowerUp **************************")]
        [Space(10)]
        [SerializeField] private GameObject shield;
        [SerializeField] private AnimationClip shieldEndAnimation;
        [SerializeField] private bool isShieldActive;
        private Animator shieldAnimator;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            shieldAnimator = shield.GetComponent<Animator>();
        }

        private void Start()
        {
            coinCollection.gameObject.SetActive(false);
            baseSpeed = playerMovement.Speed;
            rewindCoroutine = StartCoroutine(RewindCoroutine());
        }
        private void FixedUpdate()
        {
            Rewind();

            if (isCoinCollectionActive)
            {
                Collider2D[] coins = Physics2D.OverlapBoxAll(coinCollection.position, coinCollectionArea, 0);
                foreach (Collider2D collider in coins)
                {
                    if (collider.CompareTag("Coin"))
                    {
                        Debug.Log("a");
                        collider.transform.position = Vector2.MoveTowards(collider.transform.position, transform.position, coinSpeed * Time.deltaTime);
                    }
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
            yield return new WaitForSeconds(coinCollectionTime);
            isCoinCollectionActive = false;
            coinCollection.gameObject.SetActive(false);
        }

        IEnumerator ShieldCoroutine()
        {
            shieldAnimator.Play(shieldEndAnimation.name);
            yield return new WaitForSeconds(shieldEndAnimation.length);
            isShieldActive = false;
            shield.SetActive(false);
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            /////////Power Ups
            if (other.CompareTag("SpeedBoost"))
            {
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
                isCoinCollectionActive = true;
            }
            else if (other.CompareTag("Shield"))
            {
                coinCollection.gameObject.SetActive(true);
                shield.SetActive(true);
                isShieldActive = true;
            }

            /////////Obstacle
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
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(coinCollection.position, coinCollectionArea);
        }
    }
}
