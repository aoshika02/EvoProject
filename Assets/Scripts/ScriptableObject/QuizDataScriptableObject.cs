using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizDataScriptableObject", menuName = "ScriptableObjects/QuizDataScriptableObject", order = 1)]
public class QuizDataScriptableObject : ScriptableObject
{
   public List<QuizData> QuizDatas;
}
[Serializable]
public class QuizData
{
    public Sprite QuizImage;
    public Sprite AnswerImage;
    public int AnswerIndex;
}