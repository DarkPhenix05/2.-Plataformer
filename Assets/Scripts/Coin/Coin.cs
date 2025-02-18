using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float coinValue;
    [SerializeField] private AudioClip pickUP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.instance.PlaySound(pickUP);
            collision.GetComponent<Health>().AddCoins(coinValue);
            gameObject.SetActive(false);
        }
    }
}
