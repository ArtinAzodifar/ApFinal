using System;
using UnityEngine;

public class LobbyCharacterAnimations : MonoBehaviour
{
    private Animator melee;
    private Animator range;

    private void Awake()
    {
        melee = GameObject.FindWithTag("Player1").GetComponent<Animator>();
        range = GameObject.FindWithTag("Player2").GetComponent<Animator>();
    }

    public void MeleeSelect()
    {
        melee.SetTrigger("Select");
    }
    
    public void RangeSelect()
    {
        range.SetTrigger("Select");
    }

    public void StartGame()
    {
        if (CharSelector.Instance.getIsStarted())
        {
            MeleeSelect();
            RangeSelect();
        }
    }
}
