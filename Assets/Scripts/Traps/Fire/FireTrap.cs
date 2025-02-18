using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("FireTrap timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;

    [Header("FireTrap parameters")]
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float damage;

    [SerializeField] private float hitTime;

    [Header("Audio")]
    [SerializeField] private AudioClip fireAudio;

    private bool trigger;
    private bool activate;
    private Health playerHP;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        hitTime = hitTime + Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
     {
         if (collision.tag == "Player")
         {
             if (!trigger)
             {
                 StartCoroutine(Activate());
             }

            if (activate && hitTime >= 2f)
             {
                 collision.GetComponent<Health>().TakeDamage(damage);
                 hitTime = 0;
             }
         }
     }

    private IEnumerator Activate()
    {
        trigger= true;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        AudioManager.instance.PlaySound(fireAudio);
        spriteRenderer.color = Color.white;
        activate = true;
        anim.SetBool("On", true);

        yield return new WaitForSeconds(activeTime);
        activate = false;
        trigger = false;
        anim.SetBool("On", false);
        hitTime = 3;
    }
}
