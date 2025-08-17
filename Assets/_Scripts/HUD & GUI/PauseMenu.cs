using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PauseScreen : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI _pauseText;
    [SerializeField] float _fadeTime = 3f;

    private float _textAlpha;
    private bool _isPaused;
    
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        InputController.Instance.PauseEvent += OpenMenu;
        gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0;
        StartCoroutine(FadeTextInOut());
    }
   
    private IEnumerator FadeTextInOut()
    {
        float timer = 0f;
        while (_isPaused)
        {
            timer += Time.unscaledDeltaTime;

            _textAlpha = timer % _fadeTime;
            _pauseText.alpha = _textAlpha;
            yield return null;
        }
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }
    
    public void ReturnToMainMenu()
    {
        Resume();
        LevelManager.Instance.LoadMainMenu();
    }
}
