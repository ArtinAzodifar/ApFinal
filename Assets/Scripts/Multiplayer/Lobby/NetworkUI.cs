using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private GameObject LobbyPanel;
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButtonMode;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private Button clientButton;

    public void Start()
    {
        LobbyPanel.SetActive(true);
        selectPanel.SetActive(false);
        hostButton.gameObject.SetActive(true);
        clientButtonMode.gameObject.SetActive(true);
        ipInputField.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
    }

    public void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();

        StartCoroutine(PlayUIAnimation());
    }

    public void OnClientButtonModeClicked()
    {
        ipInputField.gameObject.SetActive(true);
        clientButton.gameObject.SetActive(true);
    }

    public void OnClientButtonClicked()
    {
        string serverIP = ipInputField.text;
        UnityTransport unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (unityTransport != null)
        {
            unityTransport.SetConnectionData(serverIP, 8888);
            NetworkManager.Singleton.StartClient();

            StartCoroutine(PlayUIAnimation());
        }
    }

    public IEnumerator PlayUIAnimation()
    {
        RectTransform LobbyPanelRect = LobbyPanel.GetComponent<RectTransform>();
        UIAnimationManager.Instance.HideWindow(LobbyPanelRect);
        
        yield return new WaitForSecondsRealtime(0.3f);
        
        RectTransform selectPanelRect = selectPanel.GetComponent<RectTransform>();
        UIAnimationManager.Instance.ShowWindow(selectPanelRect);
    }
}
