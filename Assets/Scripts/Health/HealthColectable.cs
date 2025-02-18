using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthColectable : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickUP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.instance.PlaySound(pickUP);
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
