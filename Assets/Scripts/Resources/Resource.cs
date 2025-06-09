using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum ResourceType { A, B }

    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int resourceAAmount;
    [SerializeField] private int resourceBAmount;

    public int ResourceAAmount => resourceAAmount;
    public int ResourceBAmount => resourceBAmount;
    public ResourceType Type => resourceType;

    public void Deplete(int amount)
    {
        if (resourceType == ResourceType.A)
        {
            resourceAAmount -= amount;
        }
        else if (resourceType == ResourceType.B)
        {
            resourceBAmount -= amount;
        }

        if (resourceAAmount <= 0 || resourceBAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}