using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class TargetSelectArrow : MonoBehaviour
{
    [SerializeField] private float heightOffset;
    [Header("Floating sine wave")]
    [SerializeField] private float floatAmplitude = 1;
    [SerializeField] private float floatFrequency = 0.5f;       //cycles per second
    [SerializeField] private float rotationSpeed = 180f;        //rotation angle per second

    private float _timer;
    private Vector3 newPosition;
    
    void Start()
    {
        newPosition = transform.position;
        newPosition.y += heightOffset;
        transform.position = newPosition;
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;

        //floating
        newPosition.y = floatAmplitude * Mathf.Sin(2*Mathf.PI*floatFrequency*_timer);   // y = Asin(2pi*freq*time)
        transform.position = newPosition;

        //rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World); 
        
    }
}
