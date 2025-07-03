using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _fadeTime = 4f;
    private float _textAlpha;
    void OnEnable()
    {
        StartCoroutine(FadeInText());
    }

    private IEnumerator FadeInText()
    {
        while (_textAlpha < 1)
        {
            _textAlpha += (Time.deltaTime / _fadeTime);
            _text.alpha = _textAlpha;
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
