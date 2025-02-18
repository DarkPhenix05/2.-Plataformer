using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float position;
    [SerializeField] private float directionX;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool canmove;
    private Rigidbody2D body;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    public float _speedR;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpAudio;

    [Header("OffJump")]
    [SerializeField] private float airtimer;//Time in air before jumping
    private float aircounter; //Time from runing off edge

    [Header("MultipleJumps")]
    [SerializeField] private int extraJumps;
    private int jumpCount;

    [Header("WalJumping")]
    [SerializeField] private float walljumpX;
    [SerializeField] private float walljumpY;

    [Header("WalPush")]
    [SerializeField] private float wallpushX;
    [SerializeField] private float wallpushY;
    [SerializeField] private float slowPush;
    [SerializeField] private float wait;

    [Header("Dash")]
    [SerializeField] private float dashpushX;
    [SerializeField] private float dashpushY;
    [SerializeField] private float slowDash;
    [SerializeField] private float waitDash;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCoolDown;
    public bool isDashing;


    private void Awake()
    {
        isDashing = false;
        dashTime = 10f;
        directionX = position;
        canmove = true;
        Time.timeScale = 1f;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        //position = transform.position.x;
        position = GameObject.FindWithTag("Player").transform.position.x;
        _speedR = GetComponent<Rigidbody2D>().velocity.x;

        //Dash cooldown
        dashTime = dashTime + Time.deltaTime;
        //print(dashTime);
        //print(position);
        if (isDashing == true && onWall() == true)
        {
            StartCoroutine(Stop());
        }

        if (onWall())
        {
            print("Wall");
            body.velocity = Vector3.zero;
            body.gravityScale = 1f;
        }

        if (canmove == true && isDashing == false)
        {
            horizontalInput = Input.GetAxis("Horizontal");

            Flip();

            //set animator parameters
            anim.SetBool("run", horizontalInput != 0);
            anim.SetBool("grounded", isGrounded());

            //NEW JUMP
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();

            }
            if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(FlorDash());
            }

            else
            {
                body.gravityScale = 7f;
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
                if (isGrounded())
                {
                    aircounter = airtimer;
                    jumpCount = extraJumps;
                    //print(jumpCount);
                    //print(extraJumps);
                }
                else
                {
                    aircounter -= Time.deltaTime;
                }
            }
        }
        else
            return;
    }

    private void Jump()
    {
        if (aircounter < 0 && !onWall() && jumpCount < 0)
        {
            return;
        }


        if (onWall())
        {
            if (horizontalInput > 0.01 || Input.GetKeyDown(KeyCode.D))
            {
                print("Right");
                WallJump();
                AudioManager.instance.PlaySound(jumpAudio);
            }
            else if (horizontalInput < -0.01 || Input.GetKeyDown(KeyCode.A))
            {
                print("Left");
                WallJump();
                AudioManager.instance.PlaySound(jumpAudio);
            }

            else
            {
                StartCoroutine("Dash");
                //print("Dash1");
            }

        }
        else
        {
            if (isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                AudioManager.instance.PlaySound(jumpAudio);
            }
            else
            {
                if (aircounter > 0 && jumpCount > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    AudioManager.instance.PlaySound(jumpAudio);
                }
                else
                {
                    if (jumpCount > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCount--;
                        AudioManager.instance.PlaySound(jumpAudio);
                        //print (jumpCount);
                    }
                }
            }
            aircounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * walljumpX, walljumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.2f, wallLayer);
        return raycastHit.collider != null;
        //Debug.DrawRay();
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
        {

        }
    }

    private void Flip()
    {
        //Flip sprite
            //print("Flip");
        if (horizontalInput > 0.01 || _speedR > 0.01)
        {
            transform.localScale = Vector3.one;
            //print("TurnR");
        }

        else if (horizontalInput < -0.01 || _speedR < -0.01)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //print("TurnL");
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        //quitas control
        canmove = false;
        Flip();
        //velocity
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallpushX, wallpushY));
        wallJumpCooldown = 0;
        Time.timeScale = slowPush;
        yield return new WaitForSecondsRealtime(wait);
        Time.timeScale = 1f;
        //Reset velocity /Optional
        body.velocity = new Vector2(0, 0);
        //Regresar control
        canmove = true;
        print("Dash");
        isDashing = false;
    }

    IEnumerator FlorDash()
    {
        if (dashTime > dashCoolDown)
        {
            isDashing = true;
            //quitas control
            canmove = false;
            Flip();
            //velocity
            if (onWall() == false)
            {
                body.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * dashpushX, dashpushY));
                wallJumpCooldown = 0;
                Time.timeScale = slowDash;
                yield return new WaitForSecondsRealtime(waitDash);

            }
            Time.timeScale = 1f;
            //Reset velocity /Optional
            body.velocity = new Vector2(0, 0);
            //Regresar control
            canmove = true;
            print("Dash");
            isDashing = false;

            dashTime = 0;
        }
    }
    IEnumerator Stop()
    {
        body.velocity = new Vector2(-100, 0);
        yield return new WaitForSecondsRealtime(15);
        print("BANG");
    }
}
