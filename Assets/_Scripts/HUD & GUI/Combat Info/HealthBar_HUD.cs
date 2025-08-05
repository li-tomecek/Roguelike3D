using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_HUD : HealthBar 
{
   [SerializeField] private Image _background;
   [SerializeField] private TextMeshProUGUI _BPText;

    public void SetTurn()
    {
        // change backgorund alpha value
    }

    public void UpdateBPText(int amount)
    {
        string txt = "BP:";
        for(int i = 0; i < amount; i++)
        {
            txt += " X";
        }
        _BPText.text = txt;
    }
}
