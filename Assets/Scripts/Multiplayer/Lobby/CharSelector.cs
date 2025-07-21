using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelector : NetworkBehaviour
{
    public NetworkList<CharacterSelectState> players;
    [SerializeField] private Character[] characters;
    public static CharSelector Instance {get; private set;}

    private void Awake()
    {
        players = new NetworkList<CharacterSelectState>();
        Debug.Log(EmailStore.Instance.GetEmail());
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
    
    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            players.OnListChanged += StateChange;
            RegisterEmailServerRpc(EmailStore.Instance.GetEmail());
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }
    
    private void OnClientConnected(ulong clientId)
    {
        players.Add(new CharacterSelectState(clientId, -1, EmailStore.Instance.GetEmail()));
    }

    private void OnClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clientID == clientId)
            {
                if (players[i].characterID != -1) {EnableCharServerRpc(players[i].characterID);}
                players.RemoveAt(i);
                break;
            }
        }
    }

    private void StateChange(NetworkListEvent<CharacterSelectState> changeEvent)
    {
        foreach (Character c in characters)
        {
            c.updateState();
        }
    }

    public void Select(int characterID)
    {
        Debug.Log("Select");
        SelectServerRpc(characterID);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectServerRpc(int characterID, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("SelectServerRpc");
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].CharacterID() == characterID) //characteri ke donbaleshim
            {
                if (characters[i].IsSelected() && characters[i].ClientID() == serverRpcParams.Receive.SenderClientId)
                {
                    characters[i].SetSelected(false, 0, "nothing");
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (players[j].clientID == characters[i].ClientID())
                        {
                            players[j] = new CharacterSelectState(players[j].clientID, -1, EmailStore.Instance.GetEmail());
                            break;
                        }
                    }
                }
                else if (!characters[i].IsSelected())
                {
                    for(int j = 0; j < players.Count; j++)
                    {
                        if(players[j].clientID == serverRpcParams.Receive.SenderClientId)
                        {
                            if(players[j].characterID != -1) {EnableCharServerRpc(players[j].characterID);}
                            characters[i].SetSelected(true, serverRpcParams.Receive.SenderClientId, players[j].email.ToString());
                            players[j] = new CharacterSelectState(players[j].clientID, characterID, players[j].email);
                            break;
                        }
                    }
                }
                break;
            }
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void RegisterEmailServerRpc(string email, ServerRpcParams rpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clientID == rpcParams.Receive.SenderClientId)
            {
                players[i] = new CharacterSelectState(players[i].clientID, players[i].characterID, email);
                break;
            }
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void EnableCharServerRpc(int characterID, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].CharacterID() == characterID)
            {
                characters[i].SetSelected(false, 0, "nothing");
                break;
            }
        }
    }
    
    public void StartGame()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].characterID == -1) return;
        }
        NetworkManager.Singleton.SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
    }
}
