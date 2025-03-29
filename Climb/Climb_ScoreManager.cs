using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Climb_ScoreManager : MonoBehaviour
{
    private float startTime;
    private float endTime;
    private bool isTimerRunning = false;

    public TextMeshProUGUI scoreText;
    public string scorePhrase = "Time : ";

    private void Start()
    {
        startTime = Time.time;
        isTimerRunning = true;
    }

    public void GoalReached()
    {
        if (isTimerRunning)
        {
            endTime = Time.time;
            isTimerRunning = false;

            float finalTime = endTime - startTime;
            Debug.Log("Goal reached in " + finalTime.ToString("F2") + " seconds.");

            if (scoreText != null)
            {
                scoreText.text = scorePhrase + finalTime.ToString("F2") + " seconds.";
            }
        }
    }
}
