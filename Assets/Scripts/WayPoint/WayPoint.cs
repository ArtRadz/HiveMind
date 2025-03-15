using System.Collections;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [Header("Waypoint Gradient Data")]
    public float distanceToResource;  // Lower means closer to the resource
    public float distanceToHive;      // Lower means closer to the hive

    [Header("Decay Settings")]
    public float initialTimeToDestruction = 5f; // Seconds until this waypoint decays

    void Start()
    {
        Debug.Log("Waypoint started");
        StartCoroutine(CountDownToDestruction(initialTimeToDestruction));
    }

    public void SetData(float resourceDistance, float hiveDistance)
    {
        distanceToResource = resourceDistance;
        distanceToHive = hiveDistance;
    }

    IEnumerator CountDownToDestruction(float timeLeft)
    {
        while (timeLeft > 0)
        {
            yield return null;
            timeLeft -= Time.deltaTime;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Waypoint triggered by: " + other.gameObject.tag);
    }
}