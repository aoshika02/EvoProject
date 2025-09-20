using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

public class TimerManager : SingletonMonoBehaviour<TimerManager>
{
    private ReactiveProperty<int> _time = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> TimeValue => _time;

    private UniTaskCompletionSource _timerCompletionSource;
    private bool _isTimerRunning = false;
    private bool _timeOver = false;
    protected override void Awake()
    {
        if (CheckInstance() == false) return;
        _timeOver = false;
        _timerCompletionSource = new UniTaskCompletionSource();
    }
    /// <summary>
    /// タイマー開始
    /// </summary>
    /// <param name="maxTime"></param>
    /// <returns></returns>
    public async UniTask Timer(int maxTime,CancellationToken token) 
    {
        //多重起動防止
        if (_isTimerRunning) return;
        _isTimerRunning = true;

        _timerCompletionSource = new UniTaskCompletionSource();
        _time.Value = maxTime;
        try
        {
            //タイマー処理
            while (_time.Value > 0)
            {
                await UniTask.WaitForSeconds(1f, cancellationToken: token);
                if (QuizManager.Instance.IsAnswer()) continue;
                _time.Value--;
            }
            //タイムアップ処理
            _timerCompletionSource.TrySetResult();
            _timerCompletionSource = new UniTaskCompletionSource();
            _isTimerRunning = false;
            _timeOver = true;
        }
        catch 
        {
            Debug.Log("タイマーをキャンセルしました");
        }
    }
    //タイムアップを待機
    public UniTask TimeOverAsync(CancellationToken token)
    {
        return UniTask.WaitUntil(() => _timeOver, cancellationToken: token);
    }
}
