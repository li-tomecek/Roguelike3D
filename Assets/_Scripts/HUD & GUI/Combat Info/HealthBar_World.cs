using UnityEngine;

public class HealthBar_World : HealthBar
{
    private void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        
        CombatManager.Instance.OnCombatStart.AddListener(() => gameObject.SetActive(true));
 
        if(gameObject.GetComponentInParent<Unit>() != null)
        {
            Unit unit = gameObject.GetComponentInParent<Unit>();
            unit.OnHealthChanged.AddListener(x => SetSliderPercent(x));
        }

        gameObject.SetActive(false);  //hide health bar until combat
    }

    private void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }
}
