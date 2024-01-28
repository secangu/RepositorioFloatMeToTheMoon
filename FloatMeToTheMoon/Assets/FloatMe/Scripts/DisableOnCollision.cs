using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace FloatMeToTheMoon
{
    public class DisableOnCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            /////////Power Ups
            if (other.CompareTag("SpeedBoost"))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("SpeedReduction"))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("Rewind"))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("OxygenRefill"))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("CoinCollection"))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.CompareTag("Shield"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
