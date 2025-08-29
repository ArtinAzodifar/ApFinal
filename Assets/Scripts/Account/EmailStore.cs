using UnityEngine;

public class EmailStore : MonoBehaviour
{
    public static EmailStore Instance;
    private string email;

    //singleton
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //setter
    public void SetEmail(string email)
    {
        this.email = email;
    }

    //getter
    public string GetEmail()
    {
        return email;
    }
}
