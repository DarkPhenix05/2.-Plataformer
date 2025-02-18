using UnityEngine;
using System.Collections;
using TMPro;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("Audio")]
    [SerializeField] private AudioClip dieAudio;
    [SerializeField] private AudioClip hurtAudio;

    [Header("InmunityFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header ("Drops")]
    [SerializeField] private bool enemy;
    private bool kill = true;
    [SerializeField] private GameObject drop;

    [SerializeField] private GameObject endGameScreen;

    [Header ("End")]
    public bool end;

    [Header ("Coins")]
    [SerializeField] private TMP_Text coinText;
    public float coins { get; private set; }

    [Header("Shots")]
    [SerializeField] private TMP_Text shotText;
    public int shots;// { get; private set; }


    private void Awake()
    {
        //set default values
        shots = 0;
        coins = 0;
        end = false;
        currentHealth = startingHealth;

        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);


        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
            AudioManager.instance.PlaySound(hurtAudio);
        } 
        else
        {
            if (!dead)
            {
                    foreach (Behaviour components in components)
                    {
                        if (enemy == true && kill ==true) 
                        {
                            Instantiate(drop, transform.position, Quaternion.identity);
                            kill = false;
                        }
                        components.enabled = false;
                    }
                anim.SetBool("grounded", true);
                anim.SetTrigger("die");
                dead = true;
                AudioManager.instance.PlaySound(dieAudio);
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invulnerability());

        foreach (Behaviour components in components)
        {
            components.enabled = true;
            shots = 15;
            shotText.text = "X" + shots.ToString();
        }
    }

    public void AddCoins(float _value)
    {
        coins = coins + _value;
        coinText.text = "X" + coins.ToString();

    }

    public void AddShots (int _valueS)
    {
        if (shots < 20)
        {
            shots = shots + _valueS;
            shotText.text = "X" + shots.ToString();
            //print("_shots");
            //shots = amo;
            print("Health shots: "+ shots);
        }
        else
        {
            shots = 30;
            shotText.text = "X" + shots.ToString();
            //print("_shots");
            //shots = amo;
            print("Health shots max: " + shots);
        }
    }

    public void TakeShots(int _valueS)
    {
        if (shots < 20)
        {
            shots = shots - _valueS;
            shotText.text = "X" + shots.ToString();
            //print("_shots");
            //shots = amo;
            print("Health shots: " + shots);
        }
    }

    public void endGame (bool _value)
    {
        end = true;
    }

    private void Update()
    {
        //shots = amo;
        if (end== true) 
        {
            endGameScreen.SetActive(true);
            Time.timeScale =0f;
        }
    }
}
