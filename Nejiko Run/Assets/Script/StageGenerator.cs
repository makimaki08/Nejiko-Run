using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30; // ステージの長さ
    int currentChipIndex;

    public Transform character; // ターゲットキャラクターの指定
    public GameObject[] stageChips; // ステージチッププレハブ配列
    public int startChipIndex; // 自動生成開始インデックス
    public int preInstantiate; // 生成先読み個数
    public List<GameObject> generatedStageList = new List<GameObject>(); // 生成ずみステージチップ保持リスト


    // Start is called before the first frame update
    void Start()
    {
        currentChipIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        // キャラクターの一から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageChipSize);
        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    // 指定のIndexまでのステージチップスを生成して、管理下に置く
    void UpdateStage(int toChipIndex)
    {
        if (toChipIndex <= currentChipIndex) return;

        // 指定のステージチップまでを作成
        for (int i = currentChipIndex+1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateState(i);

            // 生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }

        // ステージ保持上限内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();
        currentChipIndex = toChipIndex;
    }

    // 指定のインデックス位置に、Stageオブジェクトをランダムに生成
    GameObject GenerateState(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, chipIndex * StageChipSize),
            Quaternion.identity
        );
        return stageObject;
    }

    // 一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0]; // 作成ずみStageリストから、最古に作成されたStageを取得
        generatedStageList.RemoveAt(0); // リストから先頭の値を取り出す
        Destroy(oldStage); // 取り出した値を削除
    }
}
