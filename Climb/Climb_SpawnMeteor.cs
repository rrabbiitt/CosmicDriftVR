using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb_SpawnMeteor : MonoBehaviour
{
    public GameObject meteorPrefab;
    public GameObject head;
    public float minSpawnInterval = 7.0f;
    public float maxSpawnInterval = 12.0f;
    public AudioClip warningSound;

    private float nextSpawnTime;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        nextSpawnTime = GetRandomSpawnTime();
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            PlayWarningSound();
            nextSpawnTime = Time.time + GetRandomSpawnTime();
        }
    }

    private float GetRandomSpawnTime()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void PlayWarningSound()
    {
        if (warningSound != null && audioSource != null)
        {
            audioSource.clip = warningSound;
            audioSource.Play();
            StartCoroutine(SpawnRockAfterSound());
        }
    }

    private IEnumerator SpawnRockAfterSound()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        SpawnRock();
    }

    private void SpawnRock()
    {
        Vector3 headPosition = head.transform.position;
        float x = headPosition.x;
        float z = headPosition.z;
        Vector3 spawnPosition = new Vector3(x, transform.position.y, z);
        Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
    }

}
