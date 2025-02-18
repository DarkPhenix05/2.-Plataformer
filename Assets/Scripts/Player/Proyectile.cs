using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private BoxCollider2D boxColider;
    private Animator anim;
    private float lifeTime;

    private void Awake()
    {
        boxColider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); 
    }

    private void Update()
    {
        if (hit) 
        {
            return;
        }
        float movementSpeed = speed* Time.deltaTime * direction;
        transform.Translate(movementSpeed,0,0);

        lifeTime += Time.deltaTime;
        if (lifeTime > 2) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "coin") // colicion detect exept coins
        {
            hit= true;
            boxColider.enabled = false;
            anim.SetTrigger("explode"); 
        }
            print (collision.name);

        if (collision.tag== "Enemy") // if collicion was Enemy Damage it
        {
            collision.GetComponent<Health>().TakeDamage(1);
        }
    }

    public void SetDirection(float _direction)
    {
        //Set default values on call
        lifeTime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxColider.enabled = true;

        //Check direction of player to determin direction
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;

            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
