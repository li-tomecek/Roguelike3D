using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _gameOverText;
    [SerializeField] float _fadeTime = 4f;

    [SerializeField] TextMeshProUGUI _resultsLeftText;
    [SerializeField] TextMeshProUGUI _resultsRightText;
    private float _textAlpha;
    void OnEnable()
    {
        AnalyseGameStats();
        StartCoroutine(FadeInText());
    }

    private IEnumerator FadeInText()
    {
        while (_textAlpha < 1)
        {
            _textAlpha += (Time.deltaTime / _fadeTime);
            _gameOverText.alpha = _textAlpha;
            _resultsLeftText.alpha = _textAlpha;
            _resultsRightText.alpha = _textAlpha;
            yield return null;
        }
    }

    private void AnalyseGameStats()
    {
        Tuple<string, int> mostKills = new Tuple<string, int> ("", -1);
        Tuple<string, int> mostDeaths = new Tuple<string, int>("", -1);
        Tuple<string, int> mostUpgrades = new Tuple<string, int>("", -1);

        int totalRooms = 0, totalDeaths= 0;

        GameStats stats;

        foreach(PlayerUnit unit in PartyController.Instance.GetPartyMembers())
        {
            stats = unit.GetGameStats();
            if(stats.kills > mostKills.Item2)
            {
                mostKills = new Tuple<string, int>(unit.name, stats.kills);
            }
            
            if (stats.deaths > mostDeaths.Item2)
            {
                mostDeaths = new Tuple<string, int>(unit.name, stats.deaths);
            }
            
            if (stats.upgrades > mostUpgrades.Item2)
            {
                mostUpgrades = new Tuple<string, int>(unit.name, stats.upgrades);
            }

            totalDeaths += stats.deaths;
            totalRooms += stats.upgrades;
        }

        _resultsLeftText.text = $"TOTAL ROOMS: {totalRooms}\n\nTOTAL DEATHS: {totalDeaths}";
        _resultsRightText.text = $"Most Kills: {mostKills.Item1} ({mostKills.Item2})\nMost Deaths: {mostDeaths.Item1} ({mostDeaths.Item2})\nMost Upgrades: {mostUpgrades.Item1} ({mostUpgrades.Item2})";
    }

    public void ReturnToMainMenu()
    {
        LevelManager.Instance.LoadMainMenu();
    }
}
