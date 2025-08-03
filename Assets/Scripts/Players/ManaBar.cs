using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxMana(int amount)
    {
        slider.maxValue = amount;
    }
    
    public void SetMana(int amount)
    {
        slider.value = amount;
    }

    public void addMana()
    {
        if (slider.value < slider.maxValue)
        {
            slider.value++;
        }
    }

    public void FillMana()
    {
        slider.value = slider.maxValue;
    }
}