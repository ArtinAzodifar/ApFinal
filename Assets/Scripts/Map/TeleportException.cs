using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TeleportException : MonoBehaviour
{
    [SerializeField] private Transform destination1;
    [SerializeField] private Transform destination2;
    private Player1controll player1;
    private Player2Controller player2;

    void Start()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Player1"))
            {
                player1 = obj.gameObject.GetComponent<Player1controll>();
            } 
            if (obj.layer == LayerMask.NameToLayer("Player2"))
            {
                player2 = obj.gameObject.GetComponent<Player2Controller>();
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.name == "NoLightPortal")
        {
            GameObject lightObject = GameObject.Find("Global Light 2D");
            if (lightObject != null)
            {
                Light2D Light = lightObject.GetComponent<Light2D>();
                if (Light != null)
                {
                    Light.intensity = 1f;
                }
            }
        }
        if (other.CompareTag("Player1"))
        {
            Light2D Light = player1.GetComponentInChildren<Light2D>();
            Destroy(Light.gameObject);
            DestroyAllChildren();
            player1.Teleport(destination1.position);
        }

        if (other.CompareTag("Player2"))
        {
            Light2D Light = player2.GetComponentInChildren<Light2D>();
            Destroy(Light.gameObject);
            DestroyAllChildren();
            player2.Teleport(destination2.position);
        }
    }

    void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
