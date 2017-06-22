using UnityEngine;
using System.Collections;

// キャラクターを移動させる。
public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    Joystick _joystick = null;

    [SerializeField]
    float BoostSpeed = 3.0f;     // 移動速度
    [SerializeField]
    float ratateSpeed = 360.0f; // 回転速度
    [SerializeField]
    float AngleSpeed = 100.0f;  // 

    GameObject child;   // 子オブジェクト
    Rigidbody rb;       // プレイヤーのリジッドボディ
    wireManager wp;     // ライン引き
    Vector3 Center;     // 回転の軸
    Vector3 move;

    bool LineMax;
    bool BoostSet;

    // Use this for initialization
    void Start ()
    {
        // コンポーネント取得
        rb = GetComponent<Rigidbody>();
        wp = GetComponent<wireManager>();
        // 子オブジェクトを取得
        child = transform.FindChild("mainA").gameObject;

        // 初期化
        Center = Vector3.zero;
        move = Vector3.zero;
        LineMax = false;
        BoostSet = false;
    }

    // Update is called once per frame
    void Update ()
    {
        // 線を引き終わったら円運動
        if (wp.GetMax()) {
            if (!LineMax) {
                Center = wp.GetCenterPosition();
                LineMax = true;
            }

            transform.RotateAround(Center, Vector3.up, AngleSpeed * Time.deltaTime);

            child.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        }

        if (move.magnitude > 0.0f) {
            float step = ratateSpeed * Time.deltaTime;
            Quaternion myQ = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, myQ, step);
        }

        // デバック
        if (Input.GetKeyDown(KeyCode.Q)) {
            wp.WireAllDelete();
            child.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            LineMax = false;
        }
    }

    public void InitFlg()
    {
        child.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        LineMax = false;
    }

    public void Move ()
    {
        move = new Vector3(_joystick.Position.x, 0.0f, _joystick.Position.y);

        // 離したら
        if (InputManager.Instance.Ended()) {
            Boost();
        }
    }

    public void Boost()
    {
        rb.AddForce(move * BoostSpeed, ForceMode.Impulse);
        InputManager.Instance.OnTouchEnd.RemoveListener(() => Boost());
        BoostSet = false;
    }
}
