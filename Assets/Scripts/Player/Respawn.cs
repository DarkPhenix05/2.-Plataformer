using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UI_Manager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UI_Manager>();
    }

    public void RespawnCheck()
    {
        if (currentCheckpoint == null) //No check point has been trigered
        {
            uiManager.GameOver();
            return;
        }

        playerHealth.Respawn(); //Restore player health and reset animation
        transform.position = currentCheckpoint.position; //Move player to checkpoint location

        //Move the camera to the checkpoint's room
        Camera.main.GetComponent<MainCameraControler>().MoveToNewRoom(currentCheckpoint.parent);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint") //Activate CheckPoint
        {
            currentCheckpoint = collision.transform;
            AudioManager.instance.PlaySound(checkpoint);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
