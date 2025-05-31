using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<float> onTick;
    [SerializeField] private float tickTimeStep = 1f;

    void Start()
    {
        if (onTick == null)
            onTick = new UnityEvent<float>();

        StartCoroutine(FireTimeStepEvent());
    }

    void Update()
    {
        // Detect keys 1–4 and set speed accordingly
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetSpeedByKey(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetSpeedByKey(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetSpeedByKey(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetSpeedByKey(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetSpeedByKey(5);
    }

    private IEnumerator FireTimeStepEvent()
    {
        while (true)
        {
            onTick.Invoke(tickTimeStep);
            yield return new WaitForSeconds(tickTimeStep);
        }
    }

    // Map key number → new tickTimeStep value
    private void SetSpeedByKey(int keyNumber)
    {
        switch (keyNumber)
        {
            case 1:
                tickTimeStep = 1f;    // 1 s per tick
                break;
            case 2:
                tickTimeStep = 0.8f;  // 0.8 s per tick
                break;
            case 3:
                tickTimeStep = 0.5f;  // 0.5 s per tick
                break;
            case 4:
                tickTimeStep = 0.25f; // 0.25 s per tick
                break;
            case 5:
                tickTimeStep = 0.1f; // 0.1 s per tick
                break;
        }
    }
}