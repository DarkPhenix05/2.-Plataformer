using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public int _keys;
    [SerializeField] private int NeededKeys;
    [Header("Audio")]
    [SerializeField] private AudioClip openAudio;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        _keys = Number.Instance._keyNumber;

            if (_keys >= NeededKeys)
            {
                AudioManager.instance.PlaySound(openAudio);
                gameObject.SetActive(false);
            }
    }
}
