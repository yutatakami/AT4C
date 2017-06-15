using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : ObjectBase {
    List<GameObject> lineList;  // 線を引くオブジェクト
    Vector3 startPos;           // 始点の座標
    Vector3 endPos;             // 終点の座標
    int ClickCount;             // 

    [SerializeField]
    int LineNum;            // ラインの数
    [SerializeField]
    debugtext PositionText; // テキスト

	// Use this for initialization
	void Start ()
    {
        // 初期化
        lineList = new List<GameObject>();
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        ClickCount = -1;    // 最初の要素が0の為

        // タグ設定
        searchTag = Search.Tags.Line;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 仮
        {
            // タッチした時
            if (InputManager.Instance.Bigan()) {
                ClickCount++;
                // ラインオブジェクトがLineNum以上なら
                if (lineList.Count > LineNum - 1) {
                    // リスト内削除＆クリア
                    foreach (GameObject i in lineList)
                    {
						ObjectManager.ObjectPool.Instance.ReleaseGameObject(i);
                    }
                    lineList.Clear();
                    ClickCount = 0; // クリック済みな為
                }
                // 新たにゲームオブジェクト生成
                GameObject obj = new GameObject("LineSample");
				ObjectManager.ObjectPool.Instance.GetGameObject(obj, Vector3.zero, Quaternion.identity);
                // コンポーネント追加
                obj.AddComponent<LineRenderer>();
                // リストに追加
                lineList.Add(obj);
            }

            // タッチしている
            if (InputManager.Instance.Touched()) {

                // リスト内オブジェクトのコンポーネント参照
                LineRenderer renderer = lineList[ClickCount].GetComponent<LineRenderer>();

                // コンポーネントの初期化
                renderer.useWorldSpace = true;
                renderer.SetVertexCount(2);
                renderer.SetWidth(1.0f, 1.0f);

                // ラインを引く
                // 2つ目以上の線なら
                if (ClickCount > 0) {
                    renderer.SetPosition(0, startPos);
                }
                else
                {  // 1つ目の線
                   // 座標取得
                    startPos = InputManager.Instance.GetStartPos();
                    // ワールド座標変換
                    startPos.z = Mathf.Abs(Camera.main.transform.position.y);
                    startPos = Camera.main.ScreenToWorldPoint(startPos);
                    // ラインの始点に座標を設定
                    renderer.SetPosition(0, startPos);
                }
                // 座標取得
                endPos = InputManager.Instance.GetPrevPos();
                // ワールド座標変換
                endPos.z = Mathf.Abs(Camera.main.transform.position.y);
                endPos = Camera.main.ScreenToWorldPoint(endPos);
                // ラインの終点に座標を設定
                renderer.SetPosition(1, endPos);

                // 画面に座標を表示
                PositionText.SetText(ClickCount, startPos, endPos);
            }

            // 離した時
            if (InputManager.Instance.Ended()) {
                // 更新
                startPos = endPos;
            }
        }
    }
}
