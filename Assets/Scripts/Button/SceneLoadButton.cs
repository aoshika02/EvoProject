using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private SceneType _sceneType;
    private bool _loaded = false;
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
            LoadSceneAsync().Forget();
        });
    }
    //シーンの読み込み
    private async UniTask LoadSceneAsync()
    {
        if (_loaded) return;
        _loaded = true;
        await FadeManager.Instance.FadeOut();
        SoundManager.Instance.StopAllBGM();
        await SceneLoadManager.Instance.LoadSceneAsync(_sceneType);
        _loaded = false;
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
