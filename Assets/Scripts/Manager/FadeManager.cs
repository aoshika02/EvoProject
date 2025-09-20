using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private GameObject _fadeCanvas;
    private CancellationTokenSource _cancelToken = new CancellationTokenSource();
    protected override void Awake()
    {
        if (CheckInstance() == false) return;
        DontDestroyOnLoad(gameObject);
        _cancelToken = new CancellationTokenSource();
        _fadeCanvasGroup.alpha = 0f;
    }
    // フェードアウト
    public async UniTask FadeOut(float fadeTime = 0.5f)
    {
        _fadeCanvas.SetActive(true);
         await FadeAsync(1f, fadeTime);
    }
    // フェードイン
    public async UniTask FadeIn(float fadeTime = 0.5f)
    {
        await FadeAsync(0f, fadeTime);
        _fadeCanvas.SetActive(false);
    }
    /// <summary>
    /// フェード処理
    /// </summary>
    /// <param name="end">終了値</param>
    /// <param name="fadeTime">継続時間</param>
    /// <returns></returns>
    private async UniTask FadeAsync(float end, float fadeTime = 0.5f)
    {
        try
        {
            float time = 0f;
            float start = _fadeCanvasGroup.alpha;
            while (time < fadeTime)
            {
                time += Time.deltaTime;
                _fadeCanvasGroup.alpha = Mathf.Lerp(start, end, time / fadeTime);
                await UniTask.Yield(cancellationToken: _cancelToken.Token);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("FadeAsync Cancelled");
        }
        finally
        {
            _cancelToken?.Dispose();
            _cancelToken = new CancellationTokenSource();
            _fadeCanvasGroup.alpha = end;
        }
    }
}
