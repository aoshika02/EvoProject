using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotogame : MonoBehaviour
{
    void Update()
    {
        //スペースを押した時に関数呼び出し
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Invoke("loadgame()", 2.0f);
        }
    }
    /// <summary>
    /// タイトルからゲームのシーンに移行する関数
    /// </summary>
    public void loadgame() 
    {
        SceneManager.LoadScene("Main_Legacy");
    }
}
