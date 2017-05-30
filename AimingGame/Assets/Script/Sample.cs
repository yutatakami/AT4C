using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour {
    List<GameObject> lineList;  // 線を引くオブジェクト
    Vector2 startPos;           // 始点の座標
    Vector2 endPos;             // 終点の座標
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
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        ClickCount = -1;    // 最初の要素が0の為
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
                        Destroy(i);
                    }
                    lineList.Clear();
                    ClickCount = 0; // クリック済みな為
                }
                // 新たにゲームオブジェクト生成
                GameObject obj = new GameObject("LineSample");
                // コンポーネント追加
                obj.AddComponent<LineRenderer>();
                // リストに追加
                lineList.Add(obj);
                LineRenderer renderer = obj.GetComponent<LineRenderer>();
                renderer.useWorldSpace = true;
            }

            // タッチしている
            if (InputManager.Instance.Touched()) {

                // リスト内オブジェクトのコンポーネント参照
                LineRenderer renderer = lineList[ClickCount].GetComponent<LineRenderer>();

                // コンポーネントの初期化
                renderer.SetVertexCount(2);
                renderer.SetWidth(1.0f, 1.0f);

                // ラインを引く
                // 2つ目以上の線なら
                if (ClickCount > 0) {
                    renderer.SetPosition(0, startPos);
                }
                else
                { 　// 1つ目の線
                    startPos = InputManager.Instance.GetStartPos();
                    // ラインの始点に座標を設定
                    renderer.SetPosition(0, startPos);
                }
                endPos = InputManager.Instance.GetPrevPos();
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
