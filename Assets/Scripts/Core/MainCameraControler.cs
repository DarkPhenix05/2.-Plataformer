using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraControler : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    [SerializeField] private bool cameraSetings;


    private void Awake()
    {
        cameraSetings = true;
    }
    private void Update()
    {
        if (cameraSetings == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
        }

        if (cameraSetings == true)
        {
            transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
            lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            pres();
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }

    private void pres()
    {
        cameraSetings = !cameraSetings;
    }
}
