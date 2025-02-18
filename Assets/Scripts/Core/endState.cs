using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endState : MonoBehaviour
{
    [SerializeField] private bool end= false;
    [SerializeField] private AudioClip pickUP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.instance.PlaySound(pickUP);
            collision.GetComponent<Health>().endGame(end);
        }
    }
}
