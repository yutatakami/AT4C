using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public float distance = 5.0f;
    public float horizontalAngle = 0.0f;
    public float rotAngle = 180.0f;
    public float verticalAngle = 10.0f;
    public Transform lookTarget;
    public Vector3 offset = Vector3.zero;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        // カメラを位置と回転を更新する
        if (lookTarget != null) {
            Vector3 lookPosition = lookTarget.position + offset;
            // 注視対象空の相対位置を求める
            Vector3 relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0.0f) *
                new Vector3(0.0f, 0.0f, -distance);

            // 注視対象の位置にオフセット加算した位置に移動
            transform.position = lookPosition + relativePos;

            // 注視対象の注視
            transform.LookAt(lookTarget);

            // 障害物をよける
            RaycastHit hitInfo;
            if (Physics.Linecast(lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Ground")))
                transform.position = hitInfo.point;
        }
    }
}
