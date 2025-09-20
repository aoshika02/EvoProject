using UnityEngine;
using UniRx;
using TMPro;

public class TimerView : SingletonMonoBehaviour<TimerView>
{
    [SerializeField] private TextMeshProUGUI _timeText;
    private void Start()
    {
        TimerManager.Instance.TimeValue.Subscribe(time =>
        {
            UpdateTimerText(time);
        }).AddTo(this);
    }
    //タイマーの更新に応じてUIを更新
    private void UpdateTimerText(int time)
    {
        var min = time / 60;
        var sec = time % 60;

        _timeText.text = $"{min.ToString("D2")}:{sec.ToString("D2")}";
    }
}
