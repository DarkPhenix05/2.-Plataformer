using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer1 : MonoBehaviour
{
    public TMP_Text timerText;
    [SerializeField] private GameObject GameOver;
    public float maxTime;
    [SerializeField] private AudioClip hitAudio;

    void Update()
    {
        if (maxTime > 0)
        {
            maxTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(maxTime / 60);
            float second = Mathf.FloorToInt(maxTime % 60);
            timerText.text = "Timer: " + minutes.ToString("00") + ":" + second.ToString("00");
        }

        else
        {
            GameOver.SetActive(true);
            AudioManager.instance.PlaySound(hitAudio);
            Time.timeScale = 00;
        }
    }
}
