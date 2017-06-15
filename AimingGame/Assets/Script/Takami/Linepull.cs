using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linepull : MonoBehaviour
{
    public class CRendererPosition
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
    }

    public class CLine
    {
        public GameObject[] LineObject;
        public CRendererPosition[] rendererPosition;
        public bool bdir;
    }

    List<CLine> lineList;   // ラインリスト
    CLine       line;       // ラインオブジェクト
    GameObject  Pointer;    // ラインの終点についているオブジェクト(ラインオブジェクトの子)
    Vector3     vplayerPos; // 自機の座標
    float       fLimitLength;//
    float       fNowLenght; 
    int         nLNum;      // 現在ライン数
    bool        bMax;       // 

    [SerializeField]
    GameObject LinePrefab;      // ラインオブジェクト
    [SerializeField]
    int LineNum;                // ライン数
    [SerializeField]
    float Linelength;           // ラインの長さ

    // Use this for initialization
    void Start ()
    {
        // 初期化
        lineList = new List<CLine>();
        line = new CLine();
        line.LineObject = new GameObject[2];
        line.rendererPosition = new CRendererPosition[2];
        for (int i = 0; i < 2; i++) {
            line.rendererPosition[i] = new CRendererPosition();
        }
        Pointer = null;
        vplayerPos = Vector3.zero;
        nLNum = -1;
        bMax = false;
    }

    // Update is called once per frame
    void Update ()
    {
        // 自機の座標を取得
        vplayerPos = gameObject.transform.position;
        vplayerPos.y = 1.0f;

        // タッチしたら
        if (InputManager.Instance.Bigan()) {
            // ライン生成
            LineFactory();
        }

        // LineObjの更新&firstLineの始点更新
        LineUpdata(vplayerPos);

        // デバック
        if (Input.GetKeyDown(KeyCode.Q)) {
            LineAllDelete();
        }
    }

    /// <summary>
    /// ラインを生成
    /// </summary>
    void LineFactory ()
    {
        nLNum++;

        if (nLNum > LineNum - 1) {
            bMax = true;

            return;
        }

        // ラインオブジェクト生成
        line.LineObject[nLNum % 2] = ObjectManager.ObjectPool.Instance.GetGameObject(LinePrefab, Vector3.zero, Quaternion.identity);

        // リスト内オブジェクトのコンポーネント取得
        LineRenderer renderer = line.LineObject[nLNum % 2].GetComponent<LineRenderer>();
        Pointer = line.LineObject[nLNum % 2].transform.FindChild("Linepoint").gameObject;

        // ポイントのアクティブをオフ
        Pointer.SetActive(false);

        // コンポーネント初期化
        renderer.useWorldSpace = true;
        renderer.SetVertexCount(2);
        renderer.SetWidth(1.0f, 1.0f);

        // 始点の座標を設定
        if ((nLNum % 2) == 0) {
            // 始点を設定
            line.rendererPosition[nLNum % 2].StartPosition = vplayerPos;
            line.rendererPosition[nLNum % 2].StartPosition.y = 0.0f;
            // ラインの限界値を設定
            fLimitLength = Linelength;
        }
        else {
            // 始点を設定
            line.rendererPosition[nLNum % 2].StartPosition = line.rendererPosition[(nLNum % 2) - 1].EndPosition;
            // ラインの限界値を設定
            fLimitLength = fNowLenght;

            // リストに追加
            lineList.Add(line);
        }
        // ライン設定
        renderer.SetPosition(0, line.rendererPosition[nLNum % 2].StartPosition);
    }


    /// <summary>
    /// 更新
    /// </summary>
    void LineUpdata (Vector3 playerpos)
    {
        // 1つ目のラインはプレイヤーに引っ付いている
        foreach(CLine obj in lineList) {
            LineRenderer renderer = obj.LineObject[0].GetComponent<LineRenderer>();
            renderer.SetPosition(0, playerpos);
        }

        // タッチしていたらラインを引く
        if (InputManager.Instance.Touched() && !bMax) {
            LineApp();
        }
    }


    /// <summary>
    /// 時線を引く
    /// </summary>
    void LineApp ()
    {
        if (line == null) return;

        // コンポーネント取得
        LineRenderer renderer = line.LineObject[nLNum % 2].GetComponent<LineRenderer>();

        // 座標を取得しワールド座標変換
        Vector3 position = InputManager.Instance.GetPrevPos();
        position.z = Mathf.Abs(Camera.main.transform.position.y);
        position = Camera.main.ScreenToWorldPoint(position);

        // ラインの長さを求める
        fNowLenght = Vector3.Distance(line.rendererPosition[nLNum % 2].StartPosition, position);

        // ラインの長さが限界値を超えたら
        if (fNowLenght > fLimitLength) {
            float rad = Mathf.Atan2((position.z - line.rendererPosition[nLNum % 2].StartPosition.z),
                (position.x - line.rendererPosition[nLNum % 2].StartPosition.x));
            line.rendererPosition[nLNum % 2].EndPosition = new Vector3(
                line.rendererPosition[nLNum % 2].StartPosition.x + fLimitLength * Mathf.Cos(rad),
                0.0f,
                line.rendererPosition[nLNum % 2].StartPosition.z + fLimitLength * Mathf.Sin(rad));
            fNowLenght = fLimitLength;
        }
        else {
            // 終点を設定
            line.rendererPosition[nLNum % 2].EndPosition = position;
            line.rendererPosition[nLNum % 2].EndPosition.y = 0.0f;
        }

        // ラインの終点を設定
        renderer.SetPosition(1, line.rendererPosition[nLNum % 2].EndPosition);
        // ポインターを終点の座標に設定
        Pointer.transform.position = line.rendererPosition[nLNum % 2].EndPosition;
        Pointer.SetActive(true);
    }


    /// <summary>
    /// ラインを全部削除
    /// </summary>
    public void LineAllDelete ()
    {
        // タグが付いたオブジェクトリストを取得
        List<GameObject> List = ObjectManager.ObjectManager.Instance.ByTag(Search.Tags.Line);

        // リストが空
        if (List == null) return;
        // リスト内のオブジェクトを解放(削除ではなくアクティブをfalseにする)
        foreach (GameObject obj in List) {
            if (!obj.active) continue;

            // 子のオブジェクトのアクティブもfalseに
            GameObject temp = obj.transform.FindChild("Linepoint").gameObject;
            temp.SetActive(false);
			// 解放
			ObjectManager.ObjectPool.Instance.ReleaseGameObject(obj);
        }
        // 解放
        lineList.Clear();
        nLNum = -1;
        bMax = false;
    }


    /// <summary>
    /// ラインを最大値まで引いたか
    /// </summary>
    /// <returns>true:引いた　flase:まだ</returns>
    public bool GetMax()
    {
        return bMax;
    }


    /// <summary>
    /// 中心点を返す
    /// </summary>
    /// <returns>中心点</returns>
    public Vector3 GetCenterPosition()
    {
        return lineList[0].rendererPosition[0].EndPosition;
    }


    /// <summary>
    /// 終点を返す
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEndPosition()
    {
        return lineList[0].rendererPosition[1].EndPosition;
    }

    public float GetLineLenght()
    {
        return fLimitLength;
    }
}
