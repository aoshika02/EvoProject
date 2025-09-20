using UnityEngine;
using UnityEngine.UI;

public class ResutView : SingletonMonoBehaviour<ResutView>
{
    [SerializeField] private GameObject _result;
    [SerializeField] private Text _resultView;
    private void Start()
    {
        _result.SetActive(false);
    }
    //リザルトの表示
    public void Show(int succesCount)
    {
        SoundManager.Instance.PlaySE(SoundType.ViewQuizEnd);
        _resultView.text = succesCount + "問正解！";
        _result.SetActive(true);
    }
}
