using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    Vector3 startPosition;  // 始点の座標
    Vector3 prevPosition;   // 現在の座標
    Vector3 direction;      // 方向ベクトル
    float distance;         // 長さ
    bool touched;           // タッチしているか

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        // 変数の初期化
        startPosition = Vector3.zero;
        prevPosition = Vector3.zero;
        direction = Vector3.zero;
        distance = 0.0f;
        touched = false;
    }

    // Update is called once per frame
    void Update()
    {
        // デバック
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = prevPosition = Input.mousePosition;
            startPosition.z = prevPosition.z = Mathf.Abs(Camera.main.transform.position.y);
            startPosition = Camera.main.ScreenToWorldPoint(startPosition);
            prevPosition = Camera.main.ScreenToWorldPoint(prevPosition);
        }

        if (Input.GetMouseButton(0))
        {
            prevPosition = Input.mousePosition;
            prevPosition.z = Mathf.Abs(Camera.main.transform.position.y);
            prevPosition = Camera.main.ScreenToWorldPoint(prevPosition);
            direction = prevPosition - startPosition;
            distance = Vector3.Distance(prevPosition, startPosition);
        }
#else
        // タッチしたら
        if (Input.touchCount > 0)
        {
            // 初めのタッチを取得 (仮)
            Touch touch = Input.GetTouch(0);

            // タッチの状況
            switch (touch.phase)
            {
                // タッチした瞬間
                case TouchPhase.Began:
                    // 始点の座標を取得
                    startPosition = prevPosition = touch.position;
                    //startPosition.z = prevPosition.z = 303.0f;
                    //startPosition = prevPosition = Camera.main.ScreenToWorldPoint(startPosition);

                    
                    // 始点からの方向ベクトルを求める
                    direction = prevPosition - startPosition;
                    // 始点から現在の座標までの長さを求める
                    distance = Vector3.Distance(touch.position, startPosition);

                    touched = true;
                    break;

                // 動いている
                case TouchPhase.Moved:
                    // 現在の座標を取得
                    prevPosition = touch.position;
                    //prevPosition.z = 303.0f;
                    //prevPosition = Camera.main.ScreenToWorldPoint(prevPosition);

                    // 始点からの方向ベクトルを求める
                    direction = prevPosition - startPosition;
                    // 始点から現在の座標までの長さを求める
                    distance = Vector3.Distance(touch.position, startPosition);
                    break;

                // タッチしているが動いていない
                case TouchPhase.Stationary:
                    break;

                // 離れた
                case TouchPhase.Ended:
                    touched = false;
                    break;
            }
        }
#endif
    }

    /// <summary>
    /// タッチしている
    /// </summary>
    /// <returns>ture:している false:していない</returns>
    public bool Touched()
    {
#if UNITY_EDITOR
        //　デバック
        if (Input.GetMouseButton(0))
        {
            return true;
        }
        else
        {
            return false;
        }
#else
        return touched;
#endif
    }


    /// <summary>
    /// タッチした時
    /// </summary>
    /// <returns>ture:している false:していない</returns>
    public bool Bigan()
    {
        // デバック
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                return true;
        }
        return false;
#endif
    }


    /// <summary>
    /// 動いている時
    /// </summary>
    /// <returns>ture:いる false:いない</returns>
    public bool Moved()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
                return true;
        }
        return false;
    }


    /// <summary>
    /// 離した時
    /// </summary>
    /// <returns>ture:した false:していない</returns>
    public bool Ended()
    {
        // デバック
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            return true;
        }
        return false;
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                return true;
        }
        return false;
#endif
    }

    // Getter--------------------------------------------------------
    public Vector3 GetStartPos()
    {
        return startPosition;
    }

    public Vector3 GetPrevPos()
    {
        return prevPosition;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public float GetDistance()
    {
        return distance;
    }

}