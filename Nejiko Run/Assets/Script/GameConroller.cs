using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI名前空間のインポート

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
    }
    int CalcSore()
    {
        // ねじこの走行距離をスコアとする
        return (int)nejiko.transform.position.z;
    }

}
