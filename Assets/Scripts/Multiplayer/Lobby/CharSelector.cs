using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelector : NetworkBehaviour
{
    public NetworkList<CharacterSelectState> players;
    [SerializeField] private Character[] characters;
    public static CharSelector Instance { get; private set; }
    private GameManager gameManager = GameManager.Instance;

    private void Awake()
    {
        players = new NetworkList<CharacterSelectState>();
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
            StartCoroutine(setEmail());
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        UpdateAllCharactersUI();
    }

    private IEnumerator setEmail()
    {
        yield return new WaitForSeconds(1f);
        RegisterEmailServerRpc(EmailStore.Instance.GetEmail());
    }

    private void OnClientConnected(ulong clientId)
    {
        players.Add(new CharacterSelectState(clientId, -1, "test"));
    }

    private void OnClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clientID == clientId)
            {
                if (players[i].characterID != -1)
                {
                    EnableCharServerRpc(players[i].characterID);
                }
                players.RemoveAt(i);
                break;
            }
        }
        UpdateAllCharactersUI();
    }

    private void StateChange(NetworkListEvent<CharacterSelectState> changeEvent)
    {
        UpdateAllCharactersUI();
    }

    private void UpdateAllCharactersUI()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            var c = characters[i];
            bool found = false;
            for (int j = 0; j < players.Count; j++)
            {
                if (players[j].characterID == c.CharacterID())
                {
                    c.SetSelected(true, players[j].clientID, players[j].email.ToString());
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                c.SetSelected(false, 0, default);
            }
        }
    }

    public void Select(int characterID)
    {
        SelectServerRpc(characterID);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectServerRpc(int characterID, ServerRpcParams serverRpcParams = default)
    {
        ulong senderId = serverRpcParams.Receive.SenderClientId;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].CharacterID() == characterID)
            {
                if (characters[i].IsSelected() && characters[i].ClientID() == senderId)
                {
                    characters[i].SetSelected(false, 0, default);

                    for (int j = 0; j < players.Count; j++)
                    {
                        if (players[j].clientID == senderId)
                        {
                            players[j] = new CharacterSelectState(senderId, -1, players[j].email);
                            break;
                        }
                    }
                }
                else if (!characters[i].IsSelected())
                {
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (players[j].clientID == senderId)
                        {
                            if (players[j].characterID != -1)
                            {
                                EnableCharServerRpc(players[j].characterID);
                            }
                            characters[i].SetSelected(true, senderId, players[j].email.ToString());
                            players[j] = new CharacterSelectState(senderId, characterID, players[j].email);
                            break;
                        }
                    }
                }
                break;
            }
        }

        UpdateAllCharactersUI();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RegisterEmailServerRpc(string email, ServerRpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clientID == senderId)
            {
                players[i] = new CharacterSelectState(senderId, players[i].characterID, email);
                break;
            }
        }
        UpdateAllCharactersUI();
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnableCharServerRpc(int characterID, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].CharacterID() == characterID)
            {
                characters[i].SetSelected(false, 0, default);
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
        gameManager.StartGame();
    }
}
