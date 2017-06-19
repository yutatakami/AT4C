using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;    // Asset使用

public class wireManager : MonoBehaviour
{
    // レンダラー用
    public class CRendererPosition
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
    }
    // ワイヤー用
    public class CWire
    {
        public GameObject WireObj;              // ワイヤー用ゲームオブジェクト
        public GameObject Cylinder;             // 円柱
        public GameObject Point;                // ポイント
        public LightningBoltScript Bolt;        // 雷レンダラー
        public CRendererPosition rendererPos;   // レンダラー座標
    }

    List<CWire> wireList;       // ワイヤーリスト
    CWire       Wire;           // ワイヤー
    Vector3     PlayerPosition; // 自機座標
    float       LimitLeg;       // 最大値
    float       NowLeg;         // ラインの長さ
    int         LNum;           // ライン数カウント
    bool        bWireMax;       // 最大数を超えたとき
    bool        bSecondLine;    // 二つ目のライン

    [SerializeField]
    GameObject WirePrefab;  // ワイヤープレファブ
    [SerializeField]
    float BASICLINELEG; // 線の最長
    [SerializeField]
    int LineNum;        // ラインの数


	// Use this for initialization
	void Start ()
    {
        // 初期化
        wireList = new List<CWire>();
        Wire = null;
        PlayerPosition = Vector3.zero;
        LimitLeg = 0.0f;
        NowLeg = 0.0f;
        LNum = 0;
        bWireMax = false;
        bSecondLine = false;
	}

    // Update is called once per frame
    void Update ()
    {
        // 自機の座標を取得
        PlayerPosition = gameObject.transform.position;
        // タッチしたら
        if (InputManager.Instance.Bigan()) {
            // ワイヤー生成
            WireCreate();
        }

        // 引き終わったら一斉に雷を表示
        if (bWireMax) {
            foreach (CWire obj in wireList) {
                LineRenderer renderer = obj.WireObj.GetComponent<LineRenderer>();
                renderer.enabled = true;
                obj.Bolt.enabled = true;
            }
        }

        // 更新
        WireUpdata();
    }


    /// <summary>
    /// ワイヤー生成
    /// </summary>
    void WireCreate ()
    {
        // 初期化
        Wire = new CWire();
        Wire.rendererPos = new CRendererPosition();

        // ワイヤー用ゲームオブジェクト生成
        Wire.WireObj = ObjectManager.ObjectPool.Instance.GetGameObject(WirePrefab,
            Vector3.zero, Quaternion.identity);

        // コンポーネントと子オブジェクト取得
        Wire.Bolt = Wire.WireObj.GetComponent<LightningBoltScript>();
        Wire.Cylinder = Wire.WireObj.transform.FindChild("Cylinder").gameObject;
        Wire.Point = Wire.WireObj.transform.FindChild("Linepoint").gameObject;

        // 初期化
        Wire.WireObj.transform.position = Vector3.zero;
        Wire.WireObj.transform.rotation = Quaternion.identity;
        Wire.WireObj.transform.localScale = new Vector3(1f, 1f, 1f);

        Wire.Cylinder.SetActive(false);
        Wire.Point.SetActive(false);
        BoxCollider BoxCol = Wire.Point.GetComponent<BoxCollider>();
        BoxCol.enabled = false;

        if (LNum % 2 == 0) {
            // 始点設定
            Wire.rendererPos.StartPosition = PlayerPosition;
            Wire.rendererPos.StartPosition.y = 1.0f;
            // ラインの長さ設定
            LimitLeg = BASICLINELEG;
            // フラグを下す
            bSecondLine = false;
        }
        else {
            // 始点設定
            Wire.rendererPos.StartPosition = wireList[LNum - 1].rendererPos.EndPosition;
            // ラインの長さ設定
            LimitLeg = NowLeg;
            // フラグを立てる
            bSecondLine = true;
        }
        // 雷の始点設定
        Wire.Bolt.StartPosition = Wire.rendererPos.StartPosition;
    }


    /// <summary>
    /// ワイヤーオブジェクトを更新
    /// </summary>
    /// <param name="playerPos">プレイヤーの座標</param>
    void WireUpdata()
    {
        for (int i = 0; i < wireList.Count; ++i) {
            // 偶然以外は飛ばす
            if (i % 2 != 0) continue;
            // 雷の始点を自機の座標に設定
            wireList[i].Bolt.StartPosition = PlayerPosition;
        }
        // 引き終わっていたら以下は更新しない
        if (Wire == null || bWireMax) return;

        // ポイントを表示
        if (!Wire.Point.active) {
            Wire.Point.SetActive(true);
        }

        // タッチした座標を取得
        Vector3 touchPos = InputManager.Instance.GetPrevPos();
        // 線の長さを求める
        NowLeg = Vector3.Distance(Wire.rendererPos.StartPosition, touchPos);

        // 角度を求める
        float rad = Mathf.Atan2((touchPos.z - Wire.rendererPos.StartPosition.z),
            (touchPos.x - Wire.rendererPos.StartPosition.x));

        // 求めた長さが限界値を超えていたら
        if (NowLeg > LimitLeg　|| bSecondLine) {
            Wire.rendererPos.EndPosition = new Vector3(
                Wire.rendererPos.StartPosition.x + LimitLeg * Mathf.Cos(rad),
                1.0f,
                Wire.rendererPos.StartPosition.z + LimitLeg * Mathf.Sin(rad));
            NowLeg = LimitLeg;
        }
        else {
            Wire.rendererPos.EndPosition = touchPos;
            Wire.rendererPos.EndPosition.y = 1.0f;
        }

        // 雷の終点を設定
        Wire.Bolt.EndPosition = Wire.rendererPos.EndPosition;
        // ポイントを終点の位置に設定
        Vector3 pos = Wire.rendererPos.EndPosition;
        pos.y = 1.0f;
        Wire.Point.transform.position = pos;

        // 離したら
        if (InputManager.Instance.Ended()) {
            WirepostUpdata(rad);
        }
    }


    /// <summary>
    /// 後処理
    /// </summary>
    void WirepostUpdata(float rad)
    {
        // 雷を消す
        LineRenderer renderer = Wire.WireObj.GetComponent<LineRenderer>();
        renderer.enabled = false;
        Wire.Bolt.enabled = false;
        // ワイヤーを表示
        Wire.Cylinder.SetActive(true);
        // 終点の方向を向かせる
        Wire.Cylinder.transform.position = Wire.rendererPos.StartPosition;

        Quaternion defaultRotation = Wire.Cylinder.transform.localRotation;
        Wire.Cylinder.transform.LookAt(Wire.Point.transform);
        Wire.Cylinder.transform.localRotation *= defaultRotation;

        // ワイヤーを伸ばすコルーチンを呼ぶ
        StretchingWire SW = Wire.Cylinder.GetComponent<StretchingWire>();
        SW.Stretch(Wire.Cylinder, Wire.Cylinder.transform.position, LimitLeg, rad);

        // ポイントのコライダーをture
        BoxCollider BoxCol = Wire.Point.GetComponent<BoxCollider>();
        BoxCol.enabled = true;
        // プレイヤーに引っ付くワイヤーなら
        if (LNum % 2 == 0) {
            // プレイヤーの子に入れる
            Wire.WireObj.transform.parent = gameObject.transform;
        }
        // ライン数を増やす
        LNum++;
        // 最大数を超えていたら
        if (LNum > LineNum - 1) {
            bWireMax = true;
            LNum = LineNum;
        }
        // リストに追加
        wireList.Add(Wire);
    }


    /// <summary>
    /// ラインを削除
    /// </summary>
    /// <param name="id">要素数</param>
    //public void WireDelete(int id)
    //{
    //    int isActive = 0;

    //    // タグが付いたオブジェクトリストを取得
    //    List<GameObject> List = ObjectManager.ObjectManager.Instance.ByTag(Search.Tags.Line);
    //    // リストが空
    //    if (List == null) {
    //        return;
    //    }

    //    // ID番目のワイヤーオブジェクトを解放
    //    ObjectManager.ObjectPool.Instance.ReleaseGameObject(wireList[id].WireObj);

    //    // アクティブtrueのオブジェクトを数える
    //    foreach(GameObject obj in List) {
    //        if (!obj.active) continue;
    //        isActive++;
    //    }
    //    // アクティブ0だと初期化
    //    if (isActive == 0) {
    //        // プレイヤーの子に入っているワイヤーのオブジェクトをPoolに返す
    //        foreach (Transform child in transform) {
    //            if (child.tag != "wire") continue;

    //            child.parent = ObjectManager.ObjectPool.Instance.transform;
    //        }

    //        wireList.Clear();
    //        LNum = 0;
    //        bWireMax = false;
    //    }

    //}


    /// <summary>
    /// ラインを全部削除
    /// </summary>
    public void WireAllDelete ()
    {
        // タグが付いたオブジェクトリストを取得
        List<GameObject> List = ObjectManager.ObjectManager.Instance.ByTag(Search.Tags.Line);

        // リストが空
        if (List == null) return;

        // プレイヤーの子に入っているワイヤーのオブジェクトをPoolに返す
        foreach(Transform child in transform) {
            if (child.tag != "wire") continue;

            child.parent = ObjectManager.ObjectPool.Instance.transform;
        }

        // 使用したオブジェクトを初期化
        foreach(CWire wire in wireList) {
            wire.WireObj.transform.position = Vector3.zero;
            wire.WireObj.transform.rotation = Quaternion.identity;

            wire.Bolt.StartObject = null;
            wire.Bolt.EndObject = null;
            wire.Bolt.StartPosition = Vector3.zero;
            wire.Bolt.EndPosition = Vector3.zero;
            LineRenderer renderer = wire.WireObj.GetComponent<LineRenderer>();
            renderer.enabled = true;
            wire.Bolt.enabled = true;

            wire.Cylinder.transform.position = Vector3.zero;
            wire.Cylinder.transform.rotation = Quaternion.identity;
            wire.Cylinder.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            wire.Point.transform.position = Vector3.zero;

            wire.rendererPos.StartPosition = Vector3.zero;
            wire.rendererPos.EndPosition = Vector3.zero;
        }

        // リスト内のオブジェクトを解放(削除ではなくアクティブをfalseにする)
        foreach (GameObject obj in List) {
            if (!obj.active) continue;
            // 解放
            ObjectManager.ObjectPool.Instance.ReleaseGameObject(obj);
        }

        // 解放
        wireList.Clear();
        Wire = null;
        LNum = 0;
        bWireMax = false;
    }


    /// <summary>
    /// ラインを最大値まで引いたか
    /// </summary>
    /// <returns>true:引いた　flase:まだ</returns>
    public bool GetMax ()
    {
        return bWireMax;
    }


    /// <summary>
    /// 中心点を返す
    /// </summary>
    /// <returns>中心点</returns>
    public Vector3 GetCenterPosition ()
    {
        return wireList[0].rendererPos.EndPosition;
    }


    /// <summary>
    /// 線の長さを返す
    /// </summary>
    /// <returns>float 線の長さ</returns>
    public float GetLineLenght ()
    {
        return LimitLeg;
    }
}
