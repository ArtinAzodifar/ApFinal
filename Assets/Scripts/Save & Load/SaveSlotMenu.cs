using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class SaveSlotTheme
{
    public string themeName;
    public Button[] slotButtons = new Button[4];
    public TextMeshProUGUI[] slotTexts = new TextMeshProUGUI[4];
    public Button[] deleteButton = new Button[4];
    public GameObject[] emptySlotVisuals = new GameObject[4];
    public GameObject[] usedSlotVisuals = new GameObject[4];
}

public class SaveSlotMenu : MonoBehaviour
{
    public List<SaveSlotTheme> themes = new List<SaveSlotTheme>();

    void Start()
    {
        foreach (var theme in themes)
        {
            for (int i = 0; i < theme.slotButtons.Length; i++)
            {
                int slotIndex = i;
                if (theme.slotButtons[slotIndex] != null)
                {
                    theme.slotButtons[slotIndex].onClick.AddListener(() => OnSlotClicked(slotIndex));
                }

                if (theme.deleteButton != null && theme.deleteButton.Length > i && theme.deleteButton[i] != null)
                {
                    theme.deleteButton[i].onClick.AddListener(() => OnDeleteButtonClicked(slotIndex));
                }
            }
        }
        RefreshAllThemesUI();
    }

    public void RefreshAllThemesUI()
    {
        for (int i = 0; i < 4; i++)
        {
            GameData slotData = SaveManager.Instance.GetSlotData(i);

            foreach (var theme in themes)
            {
                if (slotData != null)
                {
                    if (theme.slotTexts[i] != null)
                        theme.slotTexts[i].text = $"Slot {i + 1}\nLevel: {slotData.currentLevelSceneName}";
                    
                    if (theme.usedSlotVisuals[i] != null)
                        theme.usedSlotVisuals[i].SetActive(true);
                    if (theme.emptySlotVisuals[i] != null)
                        theme.emptySlotVisuals[i].SetActive(false);
                }
                else
                {
                    if (theme.slotTexts[i] != null)
                        theme.slotTexts[i].text = "New Game";

                    if (theme.usedSlotVisuals[i] != null)
                        theme.usedSlotVisuals[i].SetActive(false);
                    if (theme.emptySlotVisuals[i] != null)
                        theme.emptySlotVisuals[i].SetActive(true);
                }
            }
        }
    }

    public void OnSlotClicked(int slotIndex)
    {
        SaveManager.Instance.SelectSlot(slotIndex);

        if (SaveManager.Instance.DoesSlotExist(slotIndex))
        {
            SaveManager.Instance.LoadGame();
        }
        else
        {
            SaveManager.Instance.NewGame();
        }
    }
    
    public void OnDeleteButtonClicked(int slotIndex)
    {
        SaveManager.Instance.DeleteSlot(slotIndex);
        RefreshAllThemesUI();
    }
}