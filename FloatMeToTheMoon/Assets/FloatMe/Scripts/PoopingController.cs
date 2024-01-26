using System.Collections;
using UnityEngine;

namespace FloatMeToTheMoon
{
    public class PoopingController : MonoBehaviour
    {
        [SerializeField] private float fallSpeed;
        private bool hit;
        Animator animator;
        [SerializeField] AnimationClip poopAnimation;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if (!hit) transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }

        IEnumerator DeactivatePoop()
        {
            animator.Play("Poop");
            hit = true;
            this.GetComponent<Collider2D>().enabled = false;

            yield return new WaitForSeconds(poopAnimation.length);
            gameObject.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            StartCoroutine(DeactivatePoop());

            if (other.CompareTag("Player"))
            {
                this.transform.SetParent(other.transform);
            }
        }
    }
}
