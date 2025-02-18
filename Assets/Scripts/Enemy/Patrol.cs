using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header ("Patroll")]
    [SerializeField] private Transform LEdge;
    [SerializeField] private Transform REdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    private Vector3 scale;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float idleTime;
    private float idleTimer;
    private bool Left;

    private void Awake()
    {
        scale = enemy.localScale;
    }

    private void OnDisable() 
    {
        anim.SetBool("Run", false);
    }

    private void Update()
    {
        if (Left) 
        { 
            if (enemy.position.x >= LEdge.position.x) 
            { 
                movement(-1);
            }
            
            else
            {
                ChangeDirection();
            }
        }
        else
        {
            if (enemy.position.x <= REdge.position.x)
            {
                movement(1);
            }

            else
            {
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    {
        anim.SetBool("Run", false);

        idleTimer += Time.deltaTime;

        if (idleTimer > idleTime)
        {
            Left = !Left;
        }
    }

    private void movement(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("Run", true);

        enemy.localScale = new Vector3 (Mathf.Abs(scale.x) * _direction, scale.y, scale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
