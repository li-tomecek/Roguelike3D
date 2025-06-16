using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
       gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }


    public void SetSliderPercent(float value)
    {
        healthSlider.value = Mathf.Clamp(value, 0, 1);
    }

    //public void SetNameText(string name)
    //{
    //    nameText.text = name;
    //}
}
