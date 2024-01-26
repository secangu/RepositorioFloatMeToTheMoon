using UnityEngine;

namespace FloatMeToTheMoon
{
    public class BirdEnemyController : MonoBehaviour
    {
        [SerializeField] GameObject poopPrefab;
        [SerializeField] Transform poopSpawn;
        [SerializeField] Vector2 direction;
        [SerializeField] private float speed;
        private float poopingTimer;

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);

            poopingTimer = -Time.deltaTime;
            if (poopingTimer <= 0)
            {
                Pooping();
                ResetPoopingTimer();
            }
        }
        private void Pooping()
        {
            Instantiate(poopPrefab, poopSpawn.position, Quaternion.identity);
        }
        private void ResetPoopingTimer()
        {
            poopingTimer = Random.Range(1, 6);
        }
    }
}
