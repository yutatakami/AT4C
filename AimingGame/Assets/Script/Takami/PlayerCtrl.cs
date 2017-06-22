using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCtrl : MonoBehaviour {

    // ステート種類
    public enum State
    {
        Boost,
        Injection,
        Died,
    }

    State state = State.Boost;      // 現在のステート
    State nextState = State.Boost;  // 次のステート

    UnityEvent onPlayerState;
    public UnityEvent OnPlayerState {
        get { return onPlayerState; }
    }

    CharacterMove charaMove;
    wireManager WireMgr;
    Rigidbody rb;
    int chengeCount;

    [SerializeField]
    int PlayerHP;                   // 体力
    [SerializeField]
    GameCamera gameCamera;          // カメラ
    [SerializeField]
    int ChengeInjection;            // 移動とライン引きを変更する
    [SerializeField]
    Vector3 BoostCameraPosition;    // 移動時のカメラ位置
    [SerializeField]
    Vector3 InjectionCameraPosition;// ライン引き時のカメラ位置

    // Use this for initialization
    void Start () {
        onPlayerState = new UnityEvent();
        charaMove = gameObject.GetComponent<CharacterMove>();
        WireMgr = gameObject.GetComponent<wireManager>();
        rb = gameObject.GetComponent<Rigidbody>();
        chengeCount = 0;
        switch (state) {
            case State.Boost:
                BoostStart();
                break;
            case State.Injection:
                InjectionStart();
                break;
        }
    }

    // Update is called once per frame
    void Update () {
        switch(state) {
            case State.Boost:
                Boosting();
                break;
            case State.Injection:
                Injectioning();
                break;
            case State.Died:
                break;
        }

        onPlayerState.Invoke();

        if (PlayerHP <= 0) {
            // 死ぬ
            Died();
        }
	}

    public void BoostStart()
    {
        Debug.Log("ブーストします");
        // ステート変更
        state = State.Boost;
        // 物理挙動をする
        rb.isKinematic = false;

        chengeCount = 0;

        EventDelete();
    }


    void Boosting()
    {
        // キャラ移動
        charaMove.Move();

        if (InputManager.Instance.Stay()) {
            chengeCount++;
        }

        if (chengeCount > ChengeInjection) {
            // カメラ移動
            Debug.Log("カメラ移動");
            gameCamera.OffsetMove(InjectionCameraPosition, InjectionStart);
        }
    }

    public void InjectionStart()
    {
        Debug.Log("ライン引きます");
        // ステート変更
        state = State.Injection;
        // 物理挙動をやめる
        rb.isKinematic = true;

        WireMgr.WireCreate();

        EventDelete();
    }


    void Injectioning()
    {
        if (!WireMgr.GetMax()) {
            if (InputManager.Instance.Bigan()) {
                WireMgr.WireCreate();
            }
        }

        if (InputManager.Instance.Ended()) {
            WireMgr.WirepostUpdata();
        }
    }


    public void Died()
    {
        state = State.Died;
        EventDelete();
    }

    /// <summary>
    /// あたり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter (Collision collision)
    {
        // 障害物と当たったら
        if (collision.transform.tag == "Object") {
            WireMgr.WireAllDelete();
        }

        // 敵と当たったら
        if (collision.transform.tag == "Enemy") {
            PlayerHP--;
        }
    }

    /// <summary>
    /// リスナーを全削除
    /// </summary>
    void EventDelete()
    {
        onPlayerState.RemoveAllListeners();
    }

    /// <summary>
    /// 現在のプレイヤーの状況を返す
    /// </summary>
    /// <returns></returns>
    public State GetState()
    {
        return state;
    }

    public Vector3 GetInjectionCameraPos ()
    {
        return InjectionCameraPosition;
    }

    public Vector3 GetBoostCameraPos ()
    {
        return BoostCameraPosition;
    }

    public float GetNowWireLength()
    {
        return WireMgr.GetNowLength();
    }
}
