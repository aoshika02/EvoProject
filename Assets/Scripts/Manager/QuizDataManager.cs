using System.Collections.Generic;
using UnityEngine;

public class QuizDataManager : SingletonMonoBehaviour<QuizDataManager>
{
    [SerializeField] private QuizDataScriptableObject _quizDatas;
    private Queue<QuizData> _quizQueue = new Queue<QuizData>();
    /// <summary>
    /// クイズデータのキューを初期化
    /// </summary>
    public void InitQueue()
    {
        _quizQueue = new Queue<QuizData>(_quizDatas.QuizDatas.Shuffle());
    }
    /// <summary>
    /// 次のクイズデータを取得
    /// </summary>
    /// <returns></returns>
    public QuizData GetNextQuizData()
    {
        if (_quizQueue.Count == 0) InitQueue();
        return _quizQueue.Dequeue();
    }
}
/// <summary>
/// リストをシャッフルする拡張メソッド
/// </summary>
public static class ShuffleExtension
{
    public static List<T> Shuffle<T>(this List<T> selfList)
    {
        if(selfList== null || selfList.Count <= 1) return new();
        List<T> resultList = new List<T>(selfList);
        for (int i = resultList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (resultList[i], resultList[j]) = (resultList[j], resultList[i]);
        }
        return resultList;
    }
}