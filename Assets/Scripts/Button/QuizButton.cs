using UnityEngine;
using UnityEngine.UI;

public class QuizButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _index;
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySE(SoundType.InputQuizSelect);
            QuizManager.Instance.QuizInput(_index);
        });
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
