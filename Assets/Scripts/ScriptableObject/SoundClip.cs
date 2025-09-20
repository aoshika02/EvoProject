using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundClip", menuName = "ScriptableObject/SoundClip")]
public class SoundClip : ScriptableObject   
{
    public List<SoundData> SoundDatas;
}
[Serializable]
public class SoundData
{
    public SoundType SoundType;
    public AudioClip AudioClip;
}