using UnityEngine;
using System.Collections;

// キャラクターを移動させる。
public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    float movement = 3.0f;  // 移動速度
    [SerializeField]
    float ratateSpeed = 2.0f;   // 回転速度
    [SerializeField]
    Vector3 Center = Vector3.zero;
    [SerializeField]
    float AngleSpeed = 100.0f;

    [SerializeField]
    float Radius = 0f;

    GameObject child;// 子オブジェクト
    Vector3 move;   // 移動方向
    Rigidbody rb;   // 
    wireManager wp; // ライン引き

    bool bfig = false;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        wp = GetComponent<wireManager>();
        child = transform.FindChild("mainA").gameObject;
    }

    // Update is called once per frame
    void Update ()
    {
        if (wp.GetMax()) {
            if (!bfig) {
                Center = wp.GetCenterPosition();
                Radius = wp.GetLineLenght();
                bfig = true;
            }

            //move = new Vector3(Radius * Mathf.Cos(Time.time), 0.0f, Radius * Mathf.Sin(Time.time)).normalized;
            //move = move * Time.deltaTime * movement;

            transform.RotateAround(Center, Vector3.up, AngleSpeed * Time.deltaTime);

            child.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        }
        else {
        }

        move = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * movement,
            0.0f, Input.GetAxis("Vertical") * Time.deltaTime * movement);

        if (move.magnitude > 0.0f) {
            float step = ratateSpeed * Time.deltaTime;
            Quaternion myQ = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, myQ, step);
        }

        // デバック
        if (Input.GetKeyDown(KeyCode.Q)) {
            wp.WireAllDelete();
            child.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            bfig = false;
        }
    }

    private void FixedUpdate ()
    {
        rb.velocity = move;
    }

    public void InitFlg()
    {
        child.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        bfig = false;
    }
}
