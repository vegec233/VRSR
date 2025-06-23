using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TrainMotion : MonoBehaviour
{
    public GameObject trainPrefab; // Drag your train prefab here in the Inspector
    private Vector3 initialPosition;
    public float speed = 2f;
    public float destroyDelay = 1f;  
    public float spawnDelay = 0.5f;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.name);
        StartCoroutine(HandleTrainCollision());
    }

    IEnumerator HandleTrainCollision()
    {
        // Delay spawning the new train
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(trainPrefab, initialPosition, Quaternion.identity);

        // Wait more (or less) before destroying this one
        yield return new WaitForSeconds(destroyDelay - spawnDelay);
        Destroy(gameObject);
    }
}
