using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _STAGE
{
    STAGE_01,
    STAGE_02,
    STAGE_03,
    STAGE_04,
    STAGE_05,

    STAGE_MAX
};

public class StageSelect : MonoBehaviour {
    public Vector3 MoveSize;
    public Vector3 MoveVec;
    public List<GameObject> StageObj;   // ステージリスト
    public int WaitFrameNum;            // 長押し判定になるまでのフレーム数
    int SelectNum;                      // 選択中のステージ

    Vector3 BasePos;
    bool TapFlag = false;               // タップしているかどうか
    int FrameCount = 0;                 // タップしてから経過したフレーム数
    Vector3 StartPos;                   // タップ開始時の座標
    Vector3 OldPosition;                // 前にタップしていた座標
    bool ScrollFlag;                    // スクロールが終わっているかどうか
    Vector3 OldScrollPos;               // スクロール開始時のベース座標


	// Use this for initialization
	void Start () {
        BasePos = StageObj[0].transform.localPosition;
        for (int i = 1; i < (int)_STAGE.STAGE_MAX; i ++)
        {
            Vector3 pos = new Vector3();
            pos.x = BasePos.x + (MoveVec.x * i);
            pos.y = BasePos.y - (MoveVec.y * i);

            StageObj[i].transform.localPosition = pos;
        }

		SelectNum = 0;
        ScrollFlag = false;
        //Debug.Log("画面中央座標：" + Screen.height / 2);
    }
	
	// Update is called once per frame
	void Update () {
        if (InputManager.Instance.Bigan())
        {
            Vector3 TouchPos = new Vector3();
            //Debug.Log("ボタン押された");
            if (!TapFlag)
            {
                RaycastHit hit;
#if UNITY_EDITOR
                TouchPos = Input.mousePosition;
                //Debug.Log(TouchPos);
                TouchPos.z = Camera.main.transform.position.z;
                Ray ray = Camera.main.ScreenPointToRay(TouchPos);
#else
                pos = Input.touches[0].position;
                pos.z = Camera.main.transform.position.z;
                Ray ray = Camera.main.ScreenPointToRay(pos);
#endif
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //Debug.Log(hit.collider.gameObject.name + "に当たった");
                    // ボタン周辺以外が押された
                    if (hit.collider.gameObject.name != "StageCanvas")
                    {
                        TapFlag = false;
                        FrameCount = 0;
                        return;
                    }

                    // ステージボタンがタップされた
                    ScrollFlag = false;
                    TapFlag = true;
                    OldPosition = TouchPos;
                    //Debug.Log(OldPosition);
                    //Debug.Log(Screen.height);
                }
                else
                {
                    //Debug.Log("なにも当たっていない");
                    return;
                }
            }
        }
        else
        {// 
            if(!TapFlag)
            {// キャンバス以外が押された
                FrameCount = 0;
            }
        }

        //// タップしているとき
        //if(InputManager.Instance.Touched())
        //{
        //    // 指定フレーム以上タップされ続けている
        //    if (TouchPos == OldPosition && FrameCount > WaitFrameNum)
        //    {
        //        TapFlag = false;
        //        FrameCount = 0;
        //        return;
        //    }

        //}

        // 指を離したとき
        if(InputManager.Instance.Ended())
        {
            //Debug.Log("ボタン離れた");
            SelectStageCheck();
            ScrollFlag = true;
            TapFlag = false;
            FrameCount = 0;
            return;
        }

        // タップしているとき
        if (InputManager.Instance.Touched())
        {
            Vector3 pointPos;
#if UNITY_EDITOR
            pointPos = Input.mousePosition;
            pointPos.z = Camera.main.transform.position.z;
            //pointPos = Camera.main.ScreenToWorldPoint(pointPos);
#else
        pointPos = Input.touches[0].position;
        pointPos.z = Camera.main.transform.position.z;
#endif

            //Debug.Log("現在座標" + pointPos);
            //Debug.Log("古い座標" + OldPosition);

            Vector3 moveVec = new Vector3();
            moveVec.y = Mathf.Abs(pointPos.y - OldPosition.y);
            if (moveVec.y > MoveVec.y)
                moveVec.y = MoveVec.y;
            moveVec.x = moveVec.y / 2;
            //Debug.Log("移動量:" + moveVec);

            for (int i = 0; i < (int)_STAGE.STAGE_MAX; i++)
            {
                Vector3 pos = StageObj[i].transform.localPosition;

                if (pos.y >= Screen.height / 2)
                {// ボタンが画面上に存在する
                    if ((pointPos.y - OldPosition.y) < 0)
                    {// 指を下へスワイプ
                        //Debug.Log("下スワイプ左下移動");

                        if (pos.y - moveVec.y <= Screen.height / 2)
                        {// 移動した後の座標が中央より下の場合
                            pos.x -= Mathf.Abs(pos.x) - Mathf.Abs(BasePos.x);
                            pos.x += moveVec.x;
                            pos.y -= moveVec.y;
                        }
                        else
                        {
                            pos.x -= moveVec.x;
                            pos.y -= moveVec.y;
                        }

                    }
                    else if ((pointPos.y - OldPosition.y) > 0)
                    {// 指を上へスワイプ
                        //Debug.Log("上スワイプ右上移動");
                        pos.x += moveVec.x;
                        pos.y += moveVec.y;
                    }
                }
                else
                {// ボタンが画面下に存在する
                    if (pointPos.y - OldPosition.y < 0)
                    {// 指を下へスワイプ
                        //Debug.Log("下スワイプ右下移動");
                        pos.x += moveVec.x;
                        pos.y -= moveVec.y;
                    }
                    else if (pointPos.y - OldPosition.y > 0)
                    {// 指を上へスワイプ
                        if (pos.y + pointPos.y - OldPosition.y > Screen.height / 2)
                        {// 移動した後の座標が画面中央より上の場合
                            pos.x -= Mathf.Abs(pos.x) - Mathf.Abs(BasePos.x);
                            pos.x += moveVec.x;
                            pos.y += moveVec.y;
                        }
                        else
                        {
                            pos.x -= moveVec.x;
                            pos.y += moveVec.y;
                        }
                        //Debug.Log("上スワイプ左上移動");
                    }
                }
                StageObj[i].transform.localPosition = pos;
            }



            FrameCount++;
            OldPosition = pointPos;
        }
        else
        {// タッチしていない
            //Debug.Log("タッチしていない");
            StageScroll();
        }
	}

    /// <summary>
    /// ステージボタンをタップした時の処理
    /// </summary>
    /// <param name="stageNum">選択したステージ番号(1～n)</param>
    public void StageTap(int stageNum)
    {
        //Debug.Log("ステージボタンをタップした時の処理");
        if (SelectNum == (stageNum - 1))
        {// 選択中のステージをタップ
            //if(stageNum < 10)
            //    Scenemanager.Instance.LoadLevel("Stage_0"+stageNum, 0.5f, 0.5f);
            //else
            //    Scenemanager.Instance.LoadLevel("Stage_" + stageNum, 0.5f, 0.5f);

            // ステージ１のみに飛ぶぞ
            Scenemanager.Instance.LoadLevel("Stage_01", 0.5f, 0.5f);
        }
        else
        {// 選択していないステージをタップ
            Vector3 TouchPos;
#if UNITY_EDITOR
            TouchPos = Input.mousePosition;
            //Debug.Log(TouchPos);
            TouchPos.z = Camera.main.transform.position.z;
            Ray ray = Camera.main.ScreenPointToRay(TouchPos);
#else
            pos = Input.touches[0].position;
            pos.z = Camera.main.transform.position.z;
            Ray ray = Camera.main.ScreenPointToRay(pos);
#endif
            //Debug.Log("現在の番号：" + SelectNum);
            //Debug.Log("移動先の番号" + (stageNum-1));
            OldScrollPos = StageObj[stageNum-1].transform.localPosition;
            SelectNum = stageNum - 1;
            ScrollFlag = true;
        }
    }

    /// <summary>
    /// 自動スクロール関数
    /// </summary>
    void StageScroll()
    {
        SelectStageCheck();

        if (!ScrollFlag)
            return;

        //Debug.Log("MoveSize = " + MoveSize);
        Vector3 moveSize = MoveSize;

        // 移動した先の座標が超える場合は移動量を調整する
        if(OldScrollPos.y >= BasePos.y)
        {// 画面上から下にスクロール
            //Debug.Log("上から下へスクロール" + StageObj[SelectNum].transform.localPosition);
            if (StageObj[SelectNum].transform.localPosition.y - moveSize.y < BasePos.y)
            {// 移動した後の座標が機銃座標を超えてしまったらそこで停止
                //Debug.Log(StageObj[SelectNum].transform.localPosition.y + "-" + moveSize.y + "<=" + BasePos.y);
                moveSize.y = Mathf.Abs(BasePos.y) - Mathf.Abs(StageObj[SelectNum].transform.localPosition.y);
                moveSize.x = moveSize.y / 2;
                ScrollFlag = false;
            }
        }
        else
        {// 画面下から上にスクロール
            //Debug.Log("下から上へスクロール" + StageObj[SelectNum].transform.localPosition);
            if (StageObj[SelectNum].transform.localPosition.y + moveSize.y > BasePos.y)
            {// 移動した後の座標が機銃座標を超えてしまったらそこで停止
                //Debug.Log(StageObj[SelectNum].transform.localPosition.y + "+" + moveSize.y + ">=" + BasePos.y);
                moveSize.y = Mathf.Abs(BasePos.y) - Mathf.Abs(StageObj[SelectNum].transform.localPosition.y);
                moveSize.x = moveSize.y / 2;
                ScrollFlag = false;
            }
        }
        //Debug.Log("moveSize = " + moveSize);

        for (int i = 0; i < (int)_STAGE.STAGE_MAX; i++)
        {
            Vector3 pos = StageObj[i].transform.localPosition;

            if (OldScrollPos.y >= BasePos.y)
            {// スクロール前のボタンが上に存在したので下にスクロール
                if (pos.y >= BasePos.y)
                {// 今のボタンが上にあるので左下移動
                    if (pos.y - moveSize.y <= BasePos.y)
                    {// 移動した後の座標が中央より下の場合
                        //pos.x -= Mathf.Abs(pos.x) - Mathf.Abs(BasePos.x);
                        pos.x = BasePos.x;
                        pos.x += moveSize.x;
                        pos.y -= moveSize.y;
                    }
                    else
                    {// 移動後もまだ中央より上
                        pos.x -= moveSize.x;
                        pos.y -= moveSize.y;
                    }
                }
                else
                {// 今のボタンが下にあるので右下移動
                    pos.x += moveSize.x;
                    pos.y -= moveSize.y;
                }
            }
            else
            {// スクロール前のボタンが下に存在したので上にスクロール
                if (pos.y >= BasePos.y)
                {// 今のボタンが上にあるので右上移動
                    pos.x += moveSize.x;
                    pos.y += moveSize.y;
                }
                else
                {// 今のボタンが下にあるので左上移動
                    if (pos.y + moveSize.y > BasePos.y)
                    {// 移動した後の座標が画面中央より上の場合右上移動に変更
                        //pos.x -= Mathf.Abs(pos.x) - Mathf.Abs(BasePos.x);
                        pos.x = BasePos.x;
                        pos.x += moveSize.x;
                        pos.y += moveSize.y;
                    }
                    else
                    {// 移動後もまだ中央より下なら左上移動を継続
                        pos.x -= moveSize.x;
                        pos.y += moveSize.y;
                    }
                }
            }
            StageObj[i].transform.localPosition = pos;
        }

        //// ベース座標より上にボタンがある
        //if(OldScrollPos.y > BasePos.y)
        //{
        //    for(int i = 0; i < (int)_STAGE.STAGE_MAX; i ++)
        //    {
        //        Vector3 pos = StageObj[i].transform.localPosition;

        //    }
        //}
        //else if(OldScrollPos.y < BasePos.y)
        //{// ベース座標より下にボタンがある
        //    for (int i = 0; i < (int)_STAGE.STAGE_MAX; i++)
        //    {

        //    }
        //}
    }

    /// <summary>
    /// 選択されているステージの決定
    /// </summary>
    void SelectStageCheck()
    {
        if (!TapFlag)
            return;
        //Debug.Log("選択されているステージの決定");
        int selectNum = 0;
        float MinSize = 1000.0f;
        // 基準座標に一番近いステージ番号を選択中にする。
        for (int i = 0; i < (int)_STAGE.STAGE_MAX; i++)
        {
            Vector3 pos = StageObj[i].transform.localPosition;
            if (Mathf.Abs(BasePos.y - pos.y) < MinSize)
            {
                MinSize = Mathf.Abs(BasePos.y - pos.y);
                selectNum = i;
            }
        }

        // 今回選択されているステージと前回選択されていたステージが違う
        if (SelectNum != selectNum)
        {
            //Debug.Log("前回ステージ：" + SelectNum + "今回ステージ：" + selectNum);
            //Debug.Log("前回座標" + StageObj[SelectNum].transform.localPosition + "今回座標" + StageObj[selectNum].transform.localPosition);
            ScrollFlag = true;
        }
        OldScrollPos = StageObj[SelectNum].transform.localPosition;
        SelectNum = selectNum;
        //Debug.Log("SelectNum = " + SelectNum);
    }
}
