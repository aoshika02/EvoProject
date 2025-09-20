using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRandom = UnityEngine.Random;
using TMPro;

public class main : MonoBehaviour
{
    //何問目かのカウント（0からカウント）
    int Quiznum;
    //正解の択
    int[] Quiz_Answers;
    //正解の択
    int[] Quiz_Real_Answers=new int[10];
    //正当数
    int nicecount;
    //問題の画像
    public GameObject[] Quiz_Q;
    //正解の画像
    public GameObject[] Quiz_A;
    //不正解の画像
    public GameObject Quiz_T;
    public GameObject Quiz_F;
    //左のボタン
    public GameObject L_Button;
    //真ん中のボタン
    public GameObject C_Button;
    //右のボタン
    public GameObject R_Button;
    //次ボタン
    public GameObject N_Button; 
    //次ボタン
    public GameObject UIsets;
    //タイマー
    public TextMeshProUGUI Timer ;
    //スコア
    public Text result ;

    //遊んでくれてありがとうの画像
    public GameObject Thankyou;
    //次の問題の表示待機判定
    bool isNext;
    //回答したかどうかのフラグ
    bool isAnswer;
    //解説を表示したかどうかのフラグ
    bool isExplanation;
    //タイトルへ戻るためのフラグ
    bool isGoTitle;
    //出題する問題の問題番号を格納
                     //1, 2, 3, 4, 5, 6, 7, 8, 9, 10
    int[] Quizindex= { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    //乱数の上限と取り出す要素数
    int start = 0;
    int end = 19;
    //制限時間
    float Times;
    int min;
    int sec;
    //クリックSE
    public AudioSource SE_1;
    //問題解説SE
    public AudioSource SE_2;
    //リザルトSE
    public AudioSource SE_3;
    
    List<int> numbers = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        //正当回数を初期化、各判定をfalseに、制限時間の初期値を300秒にする
        nicecount = 0;
        isNext = false;
        isAnswer = false;
        isExplanation = false;
        isGoTitle = false;
        Times = 300;
        //0～19の数字のリストの作成
        for (int i = start; i <= end; i++)
        {
            numbers.Add(i);           
        }
        
        int QN = 0;
        //リストの要素数が10より大きい間繰り返す
        while (numbers.Count > 10)
        {
            //配列の0～9までに0～20の数をランダムに代入
            int index = UniRandom.Range(0, numbers.Count);
            Quizindex[QN] = numbers[index];

            QN++;

            numbers.RemoveAt(index);          
        }
       
        //0の時回答1、1の時回答2、2の時回答3、が正当、4の時クイズ終了
        int[] Quiz_Answers = new int[]
        //Aが0、Bが1、Cが2
        //1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20
        { 1, 1, 2, 2, 1, 1, 1, 2, 1, 2, 1, 2, 0, 2, 2, 1, 1, 1, 2, 1 };
        //{ 0,1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19 };
        for(int Q = 0; Q < 10; Q++) 
        {
            Quiz_Real_Answers[Q] = Quiz_Answers[Quizindex[Q]];
        }       
        Allclean();
        Quiznum = 0;
        //一問目を表示
        Quiz_Q[Quizindex[Quiznum]].SetActive(true);
        Thankyou.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //回答してない間、制限時間を減少
        if (isAnswer == false) 
        {
            Times -= Time.deltaTime;
        }       
        //制限時間を分：秒になおす
        min = (int)Times / 60;
        sec = (int)Times % 60;
        //時間の表示規則
        #region Time
        if (min == 0) 
        {
            if (sec == 0)
            {
                Timer.text = "0:00";
            }
            else if (sec < 10)
            {
                Timer.text = "0:0" + sec;
            }
            else
            {
                Timer.text = "0:" + sec;
            }
        }
        else 
        {
            if (sec == 0) 
            {
                Timer.text = min + ":00" ;
            }
            else if(sec<10)
            {
                Timer.text = min + ":0" + sec;
            }
            else 
            {
                Timer.text = min + ":" + sec;
            }
        }
        #endregion
        //制限時間が0以下の場合強制的にクイズ終了
        if (Times <= 0) 
        {
            isNext = true;
            NextButton();
        }
        
        if (isAnswer == false) 
        {
            //選択肢1の処理----------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.A))
            {
                LeftButton();
            }
            //選択肢2の処理----------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.S))
            {
                centerButton();
            }
            //選択肢3の処理----------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.D))
            {
                RightButton();
            }
        }
        //回答後の処理
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                //解説を出す、次の問題を出す、タイトルに戻る　分岐
                if (isExplanation == true)
                {
                    if (isGoTitle == false) 
                    {
                        NextButton();
                    }
                    else 
                    {
                        Gototitle();
                    }
                    
                }
                else
                {
                    Explanation();
                }
            }
        }
    }  
    /// <summary>
    /// 正当の場合の関数
    /// </summary>
    public void Answer_true() 
    {
        isAnswer = true;
        nicecount++;
        Quiz_T.SetActive(true);
    }

    /// <summary>
    /// 不正答の場合の関数
    /// </summary>
    public void Answer_false() 
    {
        isAnswer = true;
        Quiz_F.SetActive(true);
    }

    /// <summary>
    /// 解説を表示し、不要なオブジェクトを非表示にするための関数
    /// </summary>
    public void Explanation() 
    {
        Quiz_T.SetActive(false);
        Quiz_F.SetActive(false);
        L_Button.SetActive(false);
        C_Button.SetActive(false);
        R_Button.SetActive(false);
        isExplanation = true;

        Quiz_Q[Quizindex[Quiznum]].SetActive(false);

        isNext = true;
       
        SE_2.Play();
        Quiz_A[Quizindex[Quiznum]].SetActive(true);
    }

    /// <summary>
    /// 左側のボタンの処理関数
    /// </summary>
    public void LeftButton()
    {
        SE_1.Play();
        //選択肢1が正答なら問題の画像を非表示にして正解の画像を表示する
        //次の問題の表示の待機中状態に移行
        if (Quiz_Real_Answers[Quiznum] == 0)
        {
            Answer_true();
        }
        //選択肢1が誤答なら問題の画像を非表示にして不正解の画像を表示する
        //次の問題の表示の待機中状態に移行
        else
        {
            Answer_false();
        }
    }
    
    /// <summary>
    /// 中央のボタンの処理関数
    /// </summary>
    public void centerButton() 
    {
        SE_1.Play();
        //選択肢2が正答なら問題の画像を非表示にして正解の画像を表示する
        //次の問題の表示の待機中状態に移行
        if (Quiz_Real_Answers[Quiznum] == 1)
        {
            Answer_true();
        }
        //選択肢2が誤答なら問題の画像を非表示にして不正解の画像を表示する
        //次の問題の表示の待機中状態に移行
        else
        {
            Answer_false();
        }
    }

    /// <summary>
    /// 右のボタンの処理関数
    /// </summary>
    public void RightButton() 
    {
        SE_1.Play();
        //選択肢3が正答なら問題の画像を非表示にして正解の画像を表示する
        //次の問題の表示の待機中状態に移行
        if (Quiz_Real_Answers[Quiznum] == 2)
        {
            Answer_true();
        }
        //選択肢3が誤答なら問題の画像を非表示にして不正解の画像を表示する
        //次の問題の表示の待機中状態に移行
        else
        {
            Answer_false();
        }
    }

    /// <summary>
    /// 全てのオブジェクトを非表示にする関数
    /// </summary>
    public void Allclean() 
    {
        //画面のクイズ関連画像を全て非表示
        for (int i = 0; i < Quiz_Q.Length; i++)
        {
            Quiz_Q[i].SetActive(false);
        }
        for (int i = 0; i < Quiz_A.Length; i++)
        {
            Quiz_A[i].SetActive(false);
        }
        Quiz_T.SetActive(false);
        Quiz_F.SetActive(false);
    }

    /// <summary>
    /// タイトルに戻る関数
    /// </summary>
    public void Gototitle() 
    {
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// 次の問題を表示するか正当回数を表示するための関数
    /// </summary>
    public void NextButton()
    {//待機状態の時のみ実行
        if (isNext == true)
        {
            Allclean();

            //次の問題の回答がないなら終わり
            //遊んでくれてありがとうの画像を表示する
            if (Quiznum == 9)
            {
                UIsets.SetActive(false);
                SE_3.Play();
                isGoTitle = true;
                Timer.text = "";
                result.text = nicecount+"問正解！";
                Thankyou.SetActive(true);
              
            }
            //次の問題の回答があるなら問題数を加算して次の問題の画像を表示
            else
            {
                L_Button.SetActive(true);
                C_Button.SetActive(true);
                R_Button.SetActive(true);
               // N_Button.SetActive(false);
                Quiznum++;
                SE_2.Play();
                Quiz_Q[Quizindex[Quiznum]].SetActive(true);
                isAnswer = false;
                isExplanation = false;
            }
        }
    }
}
