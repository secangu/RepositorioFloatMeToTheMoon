using UnityEngine;

namespace FloatMeToTheMoon
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private Vector2 direction;
        [SerializeField] private Transform minY, maxY, levelpart;
        [SerializeField] private float speed;
        [SerializeField] private float limitMapX;
        private void Start()
        {
            levelpart= transform.parent;
        }
        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);

            if (direction.x >= 1 && transform.position.x > limitMapX || direction.x >= 1 && transform.position.x > maxY.position.y)
            {
                float randomY = Random.Range(levelpart.position.y, maxY.position.y);
                transform.position = new Vector2(-limitMapX, randomY);
            }
            else if (direction.x <= 1 && transform.position.x < -limitMapX || direction.x <= 1 && transform.position.y < minY.position.y)
            {
                float randomY = Random.Range(levelpart.position.y, maxY.position.y);
                transform.position = new Vector2(limitMapX, randomY);
            }
        }
    }
}
