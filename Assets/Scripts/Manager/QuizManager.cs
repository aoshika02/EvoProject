using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class QuizManager : SingletonMonoBehaviour<QuizManager>
{
    private int _maxQuizNum = 10;
    private bool _input;
    private int _trueCount;
    private int _index;
    private bool _isAnswer;

    private QuizDataManager _quizDataManager;
    private QuizView _quizView;
    private SoundManager _soundManager;

    [SerializeField] private CanvasGroup _canvasGroup;
    public void Init(int count) 
    {
        _quizDataManager = QuizDataManager.Instance;
        _quizView = QuizView.Instance;
        _soundManager = SoundManager.Instance;

        _quizDataManager.InitQueue();
        _quizView.Init();

        _maxQuizNum = count;
        _input = false;
        _trueCount = 0;
        _index = -1;

    }
    //クイズの開始処理
    public async UniTask StartQuiz(CancellationToken token)
    {
        try
        {
            for (int i = 0; i < _maxQuizNum; i++)
            {
                _canvasGroup.blocksRaycasts = true;
                _isAnswer = false;
                var quizData = _quizDataManager.GetNextQuizData();
                _quizView.ShowQuiz(quizData);
                await WaitInput(token);
                _canvasGroup.blocksRaycasts = false;
                _isAnswer = true;
                var result = quizData.AnswerIndex == _index;
                if (result) _trueCount++;
                _quizView.ShowResult(result);
                await WaitInput(token);
                _soundManager.PlaySE(SoundType.ViewQuiz);
                _quizView.ShowAnswer(quizData);
                await WaitInput(token);
                _soundManager.PlaySE(SoundType.ViewQuiz);
            }

        }
        catch 
        {
            Debug.Log("クイズフローをキャンセルしました");
        }
    }
    public int GetTrueCount()=> _trueCount;
    public bool IsAnswer() => _isAnswer;
    //入力待機
    private async UniTask WaitInput(CancellationToken token)
    {
        _input = false;
        await UniTask.WaitUntil(() => _input,cancellationToken:token);
    }
    public void QuizInput(int index)
    {
        _input = true;
        _index = index;
    }
}
