using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _nameText;

    private void Awake()
    {
       gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        CombatManager.Instance.OnCombatStart.AddListener(() => gameObject.SetActive(true));
        
        gameObject.SetActive(false);  //hide health bar until combat
    }

    private void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }

    public void SetSliderPercent(float value)
    {
        _healthSlider.value = Mathf.Clamp(value, 0, 1);
    }

    public void SetNameText(string name)
    {
        _nameText.text = name;
    }
}
