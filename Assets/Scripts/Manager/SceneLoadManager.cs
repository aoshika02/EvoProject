using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
{
    protected override void Awake()
    {
        if (CheckInstance() == false) return;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// 非同期でシーン読み込み
    /// </summary>
    /// <param name="sceneType"></param>
    /// <returns></returns>
    public async UniTask LoadSceneAsync(SceneType sceneType)
    {
        await SceneManager.LoadSceneAsync((int)sceneType);
    }
}
public enum SceneType
{
    Title = 0,
    Quiz = 1,
}
