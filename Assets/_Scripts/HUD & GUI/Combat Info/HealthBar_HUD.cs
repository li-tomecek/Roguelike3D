using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_HUD : HealthBar 
{
   [SerializeField] private Image _background;
   [SerializeField] private TextMeshProUGUI _BPText;

    public void SetTurn(bool isTurn)
    {
        if (isTurn)
        {
            Color color = _background.color;
            color.a = 0.75f;
            
            _background.color = color;
        } else
        {
            Color color = _background.color;
            color.a = 0f;

            _background.color = color;
        }

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
