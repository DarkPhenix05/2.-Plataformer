using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Not_interactivce : MonoBehaviour
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

    [Header("Audio")]
    [SerializeField] private AudioClip talkAudio;

    void Update()
    {
        if (playerInRange && canAppear)
        {
            if (dialoguePanel.activeInHierarchy) 
            {
                if (Input.GetKeyDown(KeyCode.E) && Time.timeScale == 1 && canGoOn == true)
                {
                    dialogueText.text = "";
                    NextLine();
                }
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
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

        foreach(char letter in dialogue[index].ToCharArray())
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
        if(index != dialogue.Length)
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
            Debug.Log("Tercer E");
            dialoguePanel.SetActive(false);
            canAppear = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canAppear)
        {
            playerInRange = true;
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
        }
    }
}
