using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;
    const int MaxLife = 3;

    CharacterController controller;
    Animator animator;
    LifePanel lifePanel;
    AudioSource audioSource; // 音データの再生装置を格納する変数

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float maxSpeedZ;
    public float minSpeedZ;
    public float speedX; // 横方向スピードのパラメータ
    public float speedJump;
    public float accelerationZ; // 前進加速度のパラメータ
    public AudioClip shot;
    public AudioClip heal;
    public AudioClip hover;
    public AudioClip moteStep;


    // ライフ取得用関数
    public int Life()
    {
        return life;
    }

    // 気絶判定
    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        // 必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // 音再生装置のコンポーネント
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用
        if (Input.GetKeyDown("left")) MoveToLeft();
        if (Input.GetKeyDown("right")) MoveToRight();
        if (Input.GetKeyDown("space")) Jump();

        if (IsStun())
        {
            // 動きを止め気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            // 徐々に加速しZ方向に常に前進させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime / 30);
            moveDirection.z = Mathf.Clamp(acceleratedZ, minSpeedZ, maxSpeedZ);

            // X方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }


        // 重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime; // 重力の加算

        // 移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        // 移動後接地していたらY方向の速度はリセットする
        if (controller.isGrounded) moveDirection.y = 0;

        // 速度が0以上なら、走っているフラグをtrueにする
        animator.SetBool("run", moveDirection.z > 0.0f);

    }

    public void MoveToLeft()
    {
        if (IsStun()) return; // 気絶時の入力キャンセル
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
        // 移動時の音声を出力
        audioSource.PlayOneShot(moteStep);
    }

    public void MoveToRight()
    {
        if (IsStun()) return; // 気絶時の入力キャンセル
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
        // 移動時の音声を出力
        audioSource.PlayOneShot(moteStep);
    }

    public void Jump()
    {
        if (IsStun()) return; // 気絶時の入力キャンセル
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump; // ジャンプ

            // ジャンプトリガーを設定
            animator.SetTrigger("jump");

            // ジャンプ時の音声を出力
            audioSource.PlayOneShot(hover);
        }
    }

    // CharacterControllerに衝突判定が生じたさいの処理
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return; // 気絶時の入力キャンセル

        if(hit.gameObject.tag == "Robo")
        {
            // ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            // ダメージトリガーを設定
            animator.SetTrigger("damage");

            // 音を一度だけ再生させる
            audioSource.PlayOneShot(shot);

            // ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.tag == "Heart")
        {
            // 上限Life以下であれば、回復
            if (Life() < MaxLife)
            {
                life++;
            }
            // 回復サウンドを追加
            audioSource.PlayOneShot(heal);
            // ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }
}

