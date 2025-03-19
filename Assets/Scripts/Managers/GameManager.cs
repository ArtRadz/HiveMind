using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onTick;
    [SerializeField] private float tickTimeStep;
    // Start is called before the first frame update
    void Start()
    {
        if (onTick == null)
        {
            onTick = new UnityEvent();
        }
        StartCoroutine(FireTimeStepEvent());  
    }

    private IEnumerator FireTimeStepEvent()
    {
        int i = 0;
        while (true)
        {
            // Debug.Log($"turn {i}");
            onTick.Invoke();
            i++;
            yield return new WaitForSeconds(tickTimeStep);
        }
    }
}
