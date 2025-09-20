using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _slideObj;
    [SerializeField] private TextAsset _textAsset;
    [SerializeField] private TextMeshProUGUI _slidText;
    private async void Start()
    {
        _slideObj.SetActive(false);
        _slidText.text = _textAsset.text;
        _button.onClick.AddListener(() =>
        {
            ViewLicense();
        });
        await FadeManager.Instance.FadeIn();
    }
    private void ViewLicense()
    {
        if (_slideObj.activeSelf == false)
        {
            _slideObj.SetActive(true);
        }
        else
        {
            _slideObj.SetActive(false);
        }
    }
}
