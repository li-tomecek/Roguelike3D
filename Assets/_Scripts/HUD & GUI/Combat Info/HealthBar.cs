using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Slider _healthSlider;
    [SerializeField] protected TextMeshProUGUI _nameText;


    public void SetSliderPercent(float value)
    {
        _healthSlider.value = Mathf.Clamp(value, 0, 1);
    }

    public void SetNameText(string name)
    {
        _nameText.text = name;
    }
}
