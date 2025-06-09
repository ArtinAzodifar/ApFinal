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
        GameObject player1Tag = GameObject.FindGameObjectWithTag("Player1");
        player1 = player1Tag.GetComponent<Player1controll>();
        GameObject player2Tag = GameObject.FindGameObjectWithTag("Player2");
        player2 = player2Tag.GetComponent<Player2Controller>();
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
