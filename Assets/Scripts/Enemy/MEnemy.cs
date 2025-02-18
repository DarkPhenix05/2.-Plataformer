using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MEnemy : MonoBehaviour
{
    [Header ("Attack")]
    [SerializeField] private float Cooldown;
    [SerializeField] private int Damage;
    [SerializeField] private float Range;

    [Header("Audio")]
    [SerializeField] private AudioClip AtackAudio;

    [Header ("Detect")]
    [SerializeField] private float Distance;
    [SerializeField] private CapsuleCollider2D colider;
    private Animator anim;
    private Patrol patrol;

    [Header ("Player")]
    [SerializeField] private LayerMask player;
    private float Timer = Mathf.Infinity;
    private Health playerHP;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        patrol = GetComponentInParent<Patrol>();
    }

    private void Update()
    {
        Timer += Time.deltaTime;
            
        if(PlayerInRange())
        {
            if (Timer >= Cooldown && playerHP.currentHealth > 0)
            { 
                Timer = 0;
                anim.SetTrigger("attack");
                AudioManager.instance.PlaySound(AtackAudio);
            }
        }

        if ( patrol != null ) 
        { 
            patrol.enabled = !PlayerInRange();
        }
    }

    private bool PlayerInRange()
    {
        RaycastHit2D hit =
             Physics2D.BoxCast(colider.bounds.center + transform.right * Range * transform.localScale.x * Distance,new Vector3(colider.bounds.size.x * Range, colider.bounds.size.y, colider.bounds.size.z),0, Vector2.left, 0, player);
        if (hit.collider != null) 
        { 
            playerHP= hit.collider.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }

    private void OnDrawGizmos() //have no cllo how it does it but it shous the box cast
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(colider.bounds.center + transform.right * Range * transform.localScale.x * Distance,new Vector3(colider.bounds.size.x * Range, colider.bounds.size.y, colider.bounds.size.z));
    }

    private void GiveDamage() 
    { 
        if (PlayerInRange()) 
        {
            playerHP.TakeDamage(Damage);
        }
    }
}
