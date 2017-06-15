using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ObjectBase {

    public float speed = 6.0f;      // 移動速度
    public float jumpSpeed = 8.0f;  // ジャンプ力
    public float grabity = 20.0f;   // 重力
    public GameObject playerobj;     // プレイヤーオブジェクト
    public GameObject cameraobj;    // カメラオブジェクト

    float frad;     // 回転

    [SerializeField]
    Vector3 moveDirection;          // 移動方向

    CharacterController controller; // キャラクターコントローラー
    Linepull _linepull; // ラインを引く

	// Use this for initialization
	void Start () {
        moveDirection = Vector3.zero;
        controller = gameObject.GetComponent<CharacterController>();
        _linepull = gameObject.GetComponent<Linepull>();

        frad = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (controller.isGrounded)
        {
            // ラインを引き終わったら
            if (_linepull.GetMax()) {
                Vector3 position;
                Vector3 centerPos = _linepull.GetCenterPosition();
                Vector3 goalPos = _linepull.GetEndPosition();
                float Lenght = _linepull.GetLineLenght();

                frad += 0.5f;
                frad = frad * Mathf.Deg2Rad;

                position = new Vector3(centerPos.x + Lenght * Mathf.Cos(1 * Mathf.Deg2Rad), 
                    gameObject.transform.position.y, centerPos.z + Lenght * Mathf.Sin(1 * Mathf.Deg2Rad));
                position = Vector3.Normalize(position);

                // 入力値にカメラのオイラー格をかけることで、カメラの角度に応じた移動方向に補正する
                moveDirection = Quaternion.Euler(0.0f, cameraobj.transform.localEulerAngles.y, 0.0f) *
                    new Vector3(position.x, 0.0f, position.z);
                // 移動方向をローカルからワールド空間に変換
                moveDirection = transform.TransformDirection(moveDirection);
                // 移動速度をかける
                moveDirection *= speed;
            }
            else {
                // 入力値にカメラのオイラー格をかけることで、カメラの角度に応じた移動方向に補正する
                moveDirection = Quaternion.Euler(0.0f, cameraobj.transform.localEulerAngles.y, 0.0f) *
                    new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                // 移動方向をローカルからワールド空間に変換
                moveDirection = transform.TransformDirection(moveDirection);
                // 移動速度をかける
                moveDirection *= speed;

                if (Input.GetButton("Jump"))
                {
                    // ジャンプボタンが押下された場合、y軸方向への移動を追加
                    moveDirection.y = jumpSpeed;
                }
            }
        }

        // 移動方向に向けてキャラを回転
        if (moveDirection != Vector3.zero)
            playerobj.transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0.0f, moveDirection.y));

        // y軸方向に重力を加える
        moveDirection.y -= grabity * Time.deltaTime;
        // キャラコンを移動
        controller.Move(moveDirection * Time.deltaTime);
    }
}
