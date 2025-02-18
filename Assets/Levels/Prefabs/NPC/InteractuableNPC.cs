using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractuableNPC : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private string[] dialogue;

    [Header("Property")]
    [SerializeField] private int index;
    [SerializeField] private float speachSpeed;
    [SerializeField] private bool playerInRange;
    [SerializeField] private bool canGoOn; //give and take continue boton
    private bool canAppear = true; //apear or remove text box

    [Header("Extra")]
    [SerializeField] private GameObject propmtrE;
    private Animator anim;

    [Header("Audio")]
    [SerializeField] private AudioClip talkAudio;
    [SerializeField] private AudioClip exitAudio;
    [SerializeField] private float exitSpeachAudioTime;
    [SerializeField] private float desroyTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (playerInRange)
        {
            //propmtrE.SetActive(true);
            if (canAppear && Input.GetKeyDown(KeyCode.E))
            {
                propmtrE.SetActive(false);
                if (dialoguePanel.activeInHierarchy)
                {
                    if (Time.timeScale == 1 && canGoOn == true)
                    {
                        propmtrE.SetActive(false);
                        dialogueText.text = "";
                        NextLine();
                    }
                }
                else
                {
                    propmtrE.SetActive(false);
                    dialoguePanel.SetActive(true);
                    StartCoroutine(Typing());
                }
            }
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        canGoOn = false;
        //print ("IN");

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            if (talkAudio != null)
            {
                AudioManager.instance.PlaySound(talkAudio);
            }
            yield return new WaitForSeconds(speachSpeed);
        }

        //print ("OUT");
        canGoOn = true;
    }

    public void NextLine()
    {
        if (index != dialogue.Length)
        {
            index++;
        }
        if (index < dialogue.Length)
        {
            Debug.Log("Primer E");
            dialogueText.text = "";
            StartCoroutine(Typing());
            return;
        }

        if (index >= dialogue.Length)
        {
            StartCoroutine(Exit());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canAppear)
        {
            playerInRange = true;
            propmtrE.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            index = 0;
            canAppear = true;
            zeroText();
            propmtrE.SetActive(false);
        }
    }
    IEnumerator Exit()
    {
        Debug.Log("Tercer E");
        propmtrE.SetActive(false);
        dialoguePanel.SetActive(false);
        canAppear = false;
        AudioManager.instance.PlaySound(exitAudio);
        yield return new WaitForSeconds(exitSpeachAudioTime);
        anim.SetTrigger("OUT");
        yield return new WaitForSeconds(desroyTime);
        this.gameObject.SetActive(false);
    }
}
