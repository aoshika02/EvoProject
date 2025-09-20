using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private SoundClip _soundClip;
    private List<AudioData> _audioDatas = new List<AudioData>();
    protected override void Awake()
    {
        if (CheckInstance() == false) return;
        DontDestroyOnLoad(gameObject);
    }
    public SoundKey PlaySE(SoundType soundType)
    {
        return Play(soundType, false);
    }
    public SoundKey PlayBGM(SoundType soundType)
    {
        return Play(soundType, true);
    }
    /// <summary>
    /// BGM,SE共通の再生関数
    /// </summary>
    /// <param name="soundType"> サウンドのタイプ　</param>
    /// <param name="isLoop">ループするかどうか</param>
    /// <returns></returns>
    private SoundKey Play(SoundType soundType, bool isLoop)
    {
        var audioData = GetAudioData();
        var clip = GetAudioClip(soundType);
        if (clip == null) return null;
        audioData.AudioSource.clip = clip;
        audioData.AudioSource.loop = isLoop;
        audioData.AudioSource.Play();
        return audioData.Key;
    }
    // BGMを停止
    public void StopBGM(SoundKey key)
    {
        foreach (var ad in _audioDatas)
        {
            if (ad.AudioSource.loop == false) continue;
            if (ad.Key != key) continue;
            ad.AudioSource.Stop();
            return;
        }
    }
    // BGMを全て停止
    public void StopAllBGM()
    {
        foreach (var ad in _audioDatas)
        {
            if (ad.AudioSource.loop == false) continue;
            ad.AudioSource.Stop();
        }
    }
    // SoundTypeに対応するAudioClipを取得
    private AudioClip GetAudioClip(SoundType soundType)
    {
        return _soundClip.SoundDatas.FirstOrDefault(data => data.SoundType == soundType)?.AudioClip;
    }
    // 再生していないAudioSourceを取得
    private AudioData GetAudioData()
    {
        var soundData = _audioDatas.FirstOrDefault(x => x.IsPlaying == false);
        if (soundData == null)
        {
            return MakeAudioData();
        }
        soundData.Key = new SoundKey();
        return soundData;
    }
    // 新しいAudioSourceを作成
    private AudioData MakeAudioData()
    {
        var obj = new GameObject("AudioSource");
        obj.transform.parent = transform;
        var data = new AudioData()
        {
            SoundObj = obj,
            Key = new SoundKey(),
            AudioSource = obj.AddComponent<AudioSource>(),
        };
        data.AudioSource.playOnAwake = false;
        data.AudioSource.loop = false;
        _audioDatas.Add(data);
        return data;
    }
}
[Serializable]
public class AudioData
{
    public GameObject SoundObj;
    public SoundKey Key;
    public AudioSource AudioSource;
    public bool IsPlaying => AudioSource.isPlaying;
}