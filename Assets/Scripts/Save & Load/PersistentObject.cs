using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    public string uniqueIdInChunk;

    private string fullUniqueId;

    void Start()
    {
        fullUniqueId = $"{transform.parent.name}_{uniqueIdInChunk}";

        if (SaveManager.Instance != null && SaveManager.Instance.IsObjectProcessed(fullUniqueId))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnProcessed()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.RegisterObjectProcessed(fullUniqueId);
        }
    }
}