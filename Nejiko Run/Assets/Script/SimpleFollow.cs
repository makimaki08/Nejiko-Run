using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    Vector3 diff;

    public GameObject target; // 追従ターゲットプロパティ
    public float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        diff = target.transform.position - transform.position; // 追従距離の計算
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 線型補間関数によるスムージング
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            Time.deltaTime * followSpeed
            );
    }
}
