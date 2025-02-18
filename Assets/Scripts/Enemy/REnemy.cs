using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class REnemy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float Cooldown;
    [SerializeField] private float Range;
    [SerializeField] private int Damage;

    [Header("Audio")]
    [SerializeField] private AudioClip acidballAudio;

    [Header("R. Detect")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] acidball;


    [Header("M. Detect")]
    [SerializeField] private float Distance;
    [SerializeField] private CapsuleCollider2D colider;

    [Header("Player")]
    [SerializeField] private LayerMask player;
    private float Timer = Mathf.Infinity;

    private Animator anim;
    private Patrol patrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        patrol = GetComponentInParent<Patrol>();
    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if (PlayerInRange())
        {
            if (Timer >= Cooldown)
            {
                Timer = 0;
                anim.SetTrigger("rangedAtack");
            }
        }

        if (patrol != null)
        {
            patrol.enabled = !PlayerInRange();
        }
    }

    private bool PlayerInRange()
    {
        RaycastHit2D hit =
             Physics2D.BoxCast(colider.bounds.center + transform.right * Range * transform.localScale.x * Distance, new Vector3(colider.bounds.size.x * Range, colider.bounds.size.y, colider.bounds.size.z), 0, Vector2.left, 0, player);

        return hit.collider != null;
    }

    private void OnDrawGizmos() //have no cllo how it does it but it shous the box cast
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(colider.bounds.center + transform.right * Range * transform.localScale.x * Distance, new Vector3(colider.bounds.size.x * Range, colider.bounds.size.y, colider.bounds.size.z));
    }

    private void RangedAttack () 
    { 
        Timer = 0;
        AudioManager.instance.PlaySound(acidballAudio);
        acidball[FindFireball()].transform.position = firepoint.position;
        acidball[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < acidball.Length; i++) 
        {
            if (!acidball[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
