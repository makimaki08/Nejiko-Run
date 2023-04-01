using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI名前空間のインポート
using UnityEngine.SceneManagement;

public class GameConroller : MonoBehaviour
{
    public NejikoController nejiko;
    public Text scoreText; // ScoreTextの参照
    public LifePanel lifePanel;

    // Update is called once per frame
    void Update()
    {
        // スコアを更新
        int score = CalcSore();
        scoreText.text = "Score：" + score + "m"; // テキストの更新

        // ライフパネルを更新
        lifePanel.UpdateLife(nejiko.Life()); // LifePanelの更新

        // ねじ子のライフが0になったらゲームオーバー
        if (nejiko.Life() <= 0)
        {
            // これ以降のUpdateを止める
            enabled = false;

            // ハイスコアを更新
            if (PlayerPrefs.GetInt("HighScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            // 2秒後にReturnToTotileを呼び出す
            Invoke("ReturnToTitle", 2.0f); // 第1引数で指定した関数を、第2引数の秒数だけ遅らせて実行する

        }

    }

    int CalcSore()
    {
        // ねじこの走行距離をスコアとする
        return (int)nejiko.transform.position.z;
    }

    void ReturnToTitle()
    {
        // タイトルシーンに切り替え
        SceneManager.LoadScene("Title");
    }

}
