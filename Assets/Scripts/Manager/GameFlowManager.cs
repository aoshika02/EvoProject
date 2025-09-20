using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class GameFlowManager : SingletonMonoBehaviour<GameFlowManager>
{
    FadeManager _fadeManager;
    TimerManager _timerManager;
    ResutView _resutView;
    QuizManager _quizManager;
    SoundManager _soundManager; 
    private int _maxTime = 300;
    private int _maxQuizCount = 10;
    [SerializeField] private CanvasGroup _quizInput;
    private CancellationTokenSource _cancelTimer = new CancellationTokenSource();
    private CancellationTokenSource _cancelQuiz = new CancellationTokenSource();
    public void Start()
    {
        Flow().Forget();
    }
    private void Init()
    {
        _fadeManager = FadeManager.Instance;
        _timerManager = TimerManager.Instance;
        _resutView = ResutView.Instance;
        _quizManager = QuizManager.Instance;
        _soundManager = SoundManager.Instance;
        _cancelTimer = new CancellationTokenSource();
        _cancelQuiz = new CancellationTokenSource();
    }
    private async UniTask Flow() 
    {
        Init();
        QuizView.Instance.HideAll();
        await _fadeManager.FadeIn();
        _soundManager.PlayBGM(SoundType.InGameBGM);
        Timer(_cancelTimer.Token).Forget();
        QuizFlow(_cancelQuiz.Token).Forget();
    }
    private async UniTask QuizFlow(CancellationToken token) 
    {
        _quizManager.Init(_maxQuizCount);
        await _quizManager.StartQuiz(token);
        _cancelTimer.Cancel();
        QuizView.Instance.HideAll();
        _quizInput.blocksRaycasts = false;
        _resutView.Show(_quizManager.GetTrueCount());
    }
    private async UniTask Timer(CancellationToken token) 
    {
        _timerManager.Timer(_maxTime, token).Forget();
        await _timerManager.TimeOverAsync(token);
        _cancelQuiz.Cancel();
        QuizView.Instance.HideAll();
        _quizInput.blocksRaycasts = false;
        _resutView.Show(_quizManager.GetTrueCount());
    }
    private void OnDestroy()
    {
        _cancelQuiz?.Dispose();
        _cancelTimer?.Dispose();
    }
}
