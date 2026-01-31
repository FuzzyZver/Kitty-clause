using TMPro;
using UnityEngine;

public class RaceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _successCats;
    [SerializeField] private TextMeshProUGUI _collisionsFailed;
    [SerializeField] private TextMeshProUGUI _totalScore;
    public void Init(Race race)
    {
        _time.text = $"{race.Time.Minutes:00}:{race.Time.Seconds:00}:{race.Time.Milliseconds:000}";
        _successCats.text = race.SuccessCats.ToString();
        _collisionsFailed.text = race.CollisionsFailed.ToString();
        _totalScore.text = race.TotalScore.ToString();
    }
}
