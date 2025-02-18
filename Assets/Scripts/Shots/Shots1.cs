using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shots1 : MonoBehaviour
{
    [SerializeField] private int shotValue;
    [SerializeField] private AudioClip pickUP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.instance.PlaySound(pickUP);
            collision.GetComponent<Health>().AddShots(shotValue);
            gameObject.SetActive(false);
        }
    }
}
