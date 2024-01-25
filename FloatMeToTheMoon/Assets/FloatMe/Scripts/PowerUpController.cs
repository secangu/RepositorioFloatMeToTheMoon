using FloatMeToTheMoon.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class PowerUpController : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private AirController airController;
        private float baseSpeed;

        [Header("********************* SpeedBoost PowerUP *********************")]
        [Space(10)]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float speedBoostTime;

        [Header("********************* SpeedReduction PowerUP *****************")]
        [Space(10)]
        [SerializeField] private float minSpeed;
        [SerializeField] private float speedReductionTime;

        [Header("********************* Rewind PowerUP *************************")]
        [Space(10)]
        [SerializeField] private int rewindWaitTime;
        [SerializeField] private bool canRewind;
        [SerializeField] private bool playerHit;
        [SerializeField] private List<Vector2> positions = new List<Vector2>();
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
            airController = GetComponent<AirController>();
            shieldAnimator = shield.GetComponent<Animator>();
        }

        private void Start()
        {
            coinCollection.gameObject.SetActive(false);
            baseSpeed = playerMovement.Speed;
            rewindCoroutine = StartCoroutine(RewindCoroutine());
        }
        private void Update()
        {
            CoinCollection();
        }
        private void FixedUpdate()
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
        private void CoinCollection()
        {
            if (isCoinCollectionActive)
            {
                Collider2D[] coins = Physics2D.OverlapBoxAll(coinCollection.position, coinCollectionArea, 0);
                foreach (Collider2D collider in coins)
                {
                    if (collider.CompareTag("Coin"))
                    {
                        collider.transform.position = Vector2.MoveTowards(collider.transform.position, transform.position, coinSpeed * Time.deltaTime);
                    }
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
                float random;
                random = Random.Range(20, 50);
                airController.IncreaseAir(random);
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

                if (!canRewind && !isShieldActive && playerHit)
                {
                    airController.PlayerDied();
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
