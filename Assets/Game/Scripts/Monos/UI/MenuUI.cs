using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private DataConfig _dataConfig;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _muzikSlider;

    [Header("Races")] 
    [SerializeField] private RectTransform _racesSpawnPlace;
    [SerializeField] private List<RaceView> _raceViews = new List<RaceView>();
    private int _showRaces = 2;

    private void Awake()
    {
        int startIndex = Mathf.Max(0, _dataConfig.Races.Count - _showRaces);

        for (int i = _dataConfig.Races.Count - 1; i >= startIndex; i--)
        {
            RaceView race = Instantiate(_dataConfig.RaceView, _racesSpawnPlace);
            race.Init(_dataConfig.Races[i]);
        }
    }

    public void URL(string link)
    {
        Application.OpenURL(link);
    }

    public void SliderChangeValue(Slider slider)
    {
        if (slider == _soundSlider)
        { 
            _dataConfig.SoundVolume = slider.value;
        }
        else if (slider == _muzikSlider)
        { 
            _dataConfig.MuzikVolume = slider.value;
        }
        else Debug.LogError("Unknown setting slider");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
