using Mono.Cecil;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Canvas))]
public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private float activeTime = 2f;
    [SerializeField] private float heightOffset = 1f;
    [SerializeField] private float floatSpeed = 0.75f;
    

    public void ShowDamageAtTarget(int damage, Transform target)
    {
        _damageText.SetText(damage.ToString());
        
        gameObject.transform.position = target.position;
        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - Camera.main.transform.position);    //backwards bc for some reason the text is backwards

        StartCoroutine(Float(target));
    }

    public IEnumerator Float(Transform origin)
    {
        transform.Translate(0, heightOffset, 0);
        float timer = 0f;
        while(timer < activeTime)
        {
            //make the indicator float upwards for the active time, then deactivate the object.
            gameObject.transform.Translate(0, floatSpeed * Time.deltaTime, 0);
            yield return null;
            timer += Time.deltaTime;
        }
        
        gameObject.SetActive(false);
    }






}
