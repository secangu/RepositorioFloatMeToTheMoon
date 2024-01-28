using FloatMeToTheMoon.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class PowerUpController : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private ScoreManager scoreManager;
        private AirController airController;
        private float baseSpeed;

        [Header("********************* SpeedBoost PowerUP *********************")]
        [Space(10)]
        [SerializeField] private GameObject speedBoost;
        [SerializeField] private AnimationClip speedBoostEndAnimation;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float speedBoostTime;

        [Header("********************* SpeedReduction PowerUP *****************")]
        [Space(10)]
        [SerializeField] private GameObject slowness;
        [SerializeField] private AnimationClip slownessEndAnimation;
        [SerializeField] private float minSpeed;
        [SerializeField] private float speedReductionTime;

        [Header("********************* Rewind PowerUP *************************")]
        [Space(10)]
        [SerializeField] private GameObject rewind;
        [SerializeField] private AnimationClip rewindEndAnimation;
        [SerializeField] private int rewindWaitTime;
        [SerializeField] private bool canRewind;
        [SerializeField] private bool playerHit;
        [SerializeField] private List<Vector2> positions = new List<Vector2>();
        private Coroutine rewindCoroutine;

        [Header("******************** CoinCollection PowerUp ******************")]
        [Space(10)]
        [SerializeField] private GameObject coinAttractor;
        [SerializeField] private AnimationClip coinAttractorEndAnimation;
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

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            airController = GetComponent<AirController>();
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        private void Start()
        {
            //Desactiva Power Ups al iniciar
            speedBoost.SetActive(false);
            slowness.SetActive(false);
            rewind.SetActive(false);
            coinAttractor.SetActive(false);
            coinCollection.gameObject.SetActive(false);
            shield.SetActive(false);

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

            if (speedBoost.activeSelf) speedBoost.GetComponent<Animator>().Play(speedBoostEndAnimation.name);

            yield return new WaitForSeconds(speedBoostEndAnimation.length);
            speedBoost.SetActive(false);
            playerMovement.Speed = baseSpeed;
        }

        IEnumerator SpeedReductionCoroutine()
        {
            playerMovement.Speed = minSpeed;

            yield return new WaitForSeconds(speedReductionTime);

            if (slowness.activeSelf) slowness.GetComponent<Animator>().Play(slownessEndAnimation.name);

            yield return new WaitForSeconds(slownessEndAnimation.length);
            slowness.SetActive(false);
            playerMovement.Speed = baseSpeed;
        }

        IEnumerator RewindCoroutine()
        {
            yield return new WaitForSeconds(rewindWaitTime);

            if (rewind.activeSelf) rewind.GetComponent<Animator>().Play(rewindEndAnimation.name);

            yield return new WaitForSeconds(rewindEndAnimation.length);
            rewind.SetActive(false);
            canRewind = false;
            positions.Clear();
        }

        IEnumerator CoinCollectionCoroutine()
        {
            yield return new WaitForSeconds(coinCollectionTime);

            if (coinAttractor.activeSelf) coinAttractor.GetComponent<Animator>().Play(coinAttractorEndAnimation.name);

            yield return new WaitForSeconds(coinAttractorEndAnimation.length);

            isCoinCollectionActive = false;
            coinCollection.gameObject.SetActive(false);
            coinAttractor.SetActive(false);
        }

        IEnumerator ShieldCoroutine()
        {
            shield.GetComponent<Animator>().Play(shieldEndAnimation.name);
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
                    rewind.SetActive(false);
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
                speedBoost.SetActive(true);
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("SpeedReduction"))
            {
                StartCoroutine(SpeedReductionCoroutine());
                slowness.SetActive(true);
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("Rewind"))
            {
                StartCoroutine(RewindCoroutine());
                canRewind = true;
                rewind.SetActive(true);
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("OxygenRefill"))
            {
                float random;
                random = Random.Range(5, 20);
                airController.IncreaseAir(random);
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("CoinCollection"))
            {
                StartCoroutine(CoinCollectionCoroutine());
                isCoinCollectionActive = true;
                coinAttractor.SetActive(true);
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("Shield"))
            {
                coinCollection.gameObject.SetActive(true);
                shield.SetActive(true);
                isShieldActive = true;
                other.gameObject.SetActive(false);
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
            if (other.gameObject.CompareTag("Coin"))
            {
                scoreManager.CollectCoin();
                other.gameObject.SetActive(false);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(coinCollection.position, coinCollectionArea);
        }
    }
}
