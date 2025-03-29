using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb_Goal : MonoBehaviour
{
    public GameObject fireworkPrefab;
    public GameObject player;
    public GameObject MeteorSpawn;
    public GameObject GoalText;
    public GameObject FinalStage;
    private Climb_ScoreManager scoreManager;

    private bool hasTriggered = false;

    private void Start()
    {
        scoreManager = GameObject.Find("FunctionManager").GetComponent<Climb_ScoreManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("MainCamera"))
        {
            hasTriggered = true;
            StartCoroutine(PlayFirework());

            Vector3 centerPosition = FinalStage.transform.position;
            player.transform.position = centerPosition;

            Climb_SpawnMeteor spawnMeteorScript = MeteorSpawn.GetComponent<Climb_SpawnMeteor>();
            if (spawnMeteorScript != null)
            {
                spawnMeteorScript.enabled = false;
            }

            if (scoreManager != null)
            {
                scoreManager.GoalReached();
            }

            if (GoalText != null)
            {
                GoalText.SetActive(true);
            }
        }
    }

    private System.Collections.IEnumerator PlayFirework()
    {
        fireworkPrefab.SetActive(true);
        yield return new WaitForSeconds(20f);
        fireworkPrefab.SetActive(false);
    }
}
