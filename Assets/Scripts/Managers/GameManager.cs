using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Handles the simulation's tick loop and speed control. 
    /// Note: UI and input handling in this script are temporary placeholders for manual testing.
    /// </summary>
    public UnityEvent<float> onTick;

    [SerializeField] private float tickTimeStep = 1f;
    [SerializeField] private TextMeshProUGUI speedIndicator;

    private void Start()
    {
        if (onTick == null)
        {
            onTick = new UnityEvent<float>();
        }

        StartCoroutine(FireTimeStepEvent());
    }

    private void Update()
    {
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

    private void SetSpeedByKey(int keyNumber)
    {
        switch (keyNumber)
        {
            case 1:
                tickTimeStep = 1f;
                speedIndicator.text = "1";
                break;
            case 2:
                tickTimeStep = 0.8f;
                speedIndicator.text = "2";
                break;
            case 3:
                tickTimeStep = 0.5f;
                speedIndicator.text = "3";
                break;
            case 4:
                tickTimeStep = 0.25f;
                speedIndicator.text = "4";
                break;
            case 5:
                tickTimeStep = 0.1f;
                speedIndicator.text = "5";
                break;
        }
    }
}