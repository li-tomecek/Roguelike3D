using UnityEngine;
using UnityEngine.UIElements;

public class Party_HUD : MonoBehaviour
{
    [SerializeField] private GameObject _elementPrefab;
    [SerializeField] private Transform _parentPanel;

    public void SetupElement(PlayerUnit unit)
    {
        HealthBar_HUD healthBar = Instantiate(_elementPrefab, _parentPanel).GetComponent<HealthBar_HUD>();
        unit.OnHealthChanged.AddListener(healthBar.SetSliderPercent);
        unit.OnBPChanged.AddListener(healthBar.UpdateBPText);

        healthBar.SetNameText(unit.name);
        healthBar.gameObject.SetActive(true);
    }
}
