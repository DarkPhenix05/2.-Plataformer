using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrow;
    private float cooldownTimer;

    [Header("Audio")]
    [SerializeField] private AudioClip arowAudio;

    private void Shoot()
    {
        AudioManager.instance.PlaySound(arowAudio);
        cooldownTimer = 0;
        arrow[FindArrow()].transform.position = firePoint.position;
        arrow[FindArrow()].GetComponent<EnemyArrow>().SetArrow();
    }

    private int FindArrow()
    {
        
        for (int i = 0; i < arrow.Length; i++) 
        { 
            if (!arrow[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
            Shoot();
    }
}
