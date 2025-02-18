using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;
    [SerializeField] private AudioClip fireBallAudio;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    //private bool pause;

    public int shots;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        //pause = false;
    }

    private void Update()
    {
        shots = GetComponent<Health>().shots;
        if (shots > 0)  //no se pq no esta tomando el valor de shots.
        {
            if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && playerMovement.canAttack() && Time.timeScale == 1.0f)
            {
                Attack();
                GetComponent<Health>().TakeShots(1);
            }
            cooldownTimer += Time.deltaTime;
        } 

        /*if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && playerMovement.canAttack() && Time.timeScale == 1.0f)
        {
            //print(_shots);
            print("Cant");
        }*/

        //print("Attack shots: "+ _shots);
    } 
    private void Attack()
    {
        cooldownTimer = 0;
        anim.SetTrigger("attack");
        AudioManager.instance.PlaySound(fireBallAudio);

        fireBalls[FindFireBall()].transform.position = firePoint.position;
        fireBalls[FindFireBall()].GetComponent<Proyectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindFireBall()
    {
        for (int i = 0; i<fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
