using UnityEngine;
using UnityEngine.UI;

public class QuizView : SingletonMonoBehaviour<QuizView>
{
    [SerializeField] private Image _quizView;
    [SerializeField] private GameObject _quizTrue;
    [SerializeField] private GameObject _quizFalse;
    [SerializeField] private Image _answerView;

    public void Init()
    {
        HideAll();
    }
    //問題文を表示
    public void ShowQuiz(QuizData quizData)
    {
        _answerView.gameObject.SetActive(false);
        _quizView.sprite = quizData.QuizImage;
        _quizView.gameObject.SetActive(true);
    }
    //クイズの判定を表示
    public void ShowResult(bool result)
    {
        _quizView.gameObject.SetActive(false);
        if (result)
        {
            _quizTrue.SetActive(true);
        }
        else
        {
            _quizFalse.SetActive(true);
        }
    }
    //クイズの解説を表示
    public void ShowAnswer(QuizData quizData)
    {
        _quizTrue.SetActive(false);
        _quizFalse.SetActive(false);
        _answerView.sprite = quizData.AnswerImage;
        _answerView.gameObject.SetActive(true);
    }
    public void HideAll() 
    {
        _quizView.gameObject.SetActive(false);
        _quizTrue.SetActive(false);
        _quizFalse.SetActive(false);
        _answerView.gameObject.SetActive(false);
    }
}
