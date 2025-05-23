using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<float> onTick;
    [SerializeField] private float tickTimeStep;
    // Start is called before the first frame update
    void Start()
    {
        if (onTick == null)
        {
            onTick = new UnityEvent<float>();
        }
        StartCoroutine(FireTimeStepEvent());  
    }

    private IEnumerator FireTimeStepEvent()
    {
        while (true)
        {
            onTick.Invoke(tickTimeStep);
            yield return new WaitForSeconds(tickTimeStep);
        }
    }
}
