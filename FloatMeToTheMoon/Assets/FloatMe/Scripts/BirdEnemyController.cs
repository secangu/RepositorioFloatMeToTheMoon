using System.Collections.Generic;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class BirdEnemyController : MonoBehaviour
    {
        [SerializeField] GameObject poopPrefab;
        [SerializeField] Transform poopSpawn;
        [SerializeField] Vector2 direction;
        [SerializeField] private float speed;
        [SerializeField] private int maxPoops;
        [SerializeField] private float poopingTimer;

        [SerializeField] private List<GameObject> poops = new List<GameObject>();

        private void Start()
        {

        }

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);

            poopingTimer -= Time.deltaTime;
            if (poopingTimer <= 0)
            {
                if (poops.Count < maxPoops)
                {
                    CreatePoop();
                }
                else
                {
                    Pooping();
                }
                ResetPoopingTimer();
            }
        }

        private void Pooping()
        {
            // Buscar un "poop" disponible en la lista
            GameObject poop = poops.Find(p => !p.activeSelf);

            if (poop == null)
            {
                // Si no hay instancias inactivas, no hacer nada
                return;
            }

            // Activar el "poop" y configurar su posición
            poop.SetActive(true);
            poop.transform.position = poopSpawn.position;
        }

        private void ResetPoopingTimer()
        {
            poopingTimer = Random.Range(1, 6);
        }

        private GameObject CreatePoop()
        {
            // Crear una nueva instancia de "poop" y agregarla a la lista
            GameObject poop = Instantiate(poopPrefab, poopSpawn.position, Quaternion.identity);
            poops.Add(poop);
            return poop;
        }
    }
}
