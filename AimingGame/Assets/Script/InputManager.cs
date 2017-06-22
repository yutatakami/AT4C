using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    UnityEvent onTouchBegin;
    public UnityEvent OnTouchBegin
    {
        get { return onTouchBegin; }
    }

    UnityEvent onTouchMove;
    public UnityEvent OnTouchMove
    {
        get { return onTouchMove; }
    }

    UnityEvent onTouchStay;
    public UnityEvent OnTouchStay
    {
        get { return onTouchStay; }
    }

    UnityEvent onTouchEnd;
    public UnityEvent OnTouchEnd
    {
        get { return onTouchEnd; }
    }

    Vector2 startPosition;  // 始点の座標
    Vector2 prevPosition;   // 現在の座標
    Vector2 direction;      // 方向ベクトル
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

        // イベントの初期化
        onTouchBegin = new UnityEvent();
        onTouchMove = new UnityEvent();
        onTouchStay = new UnityEvent();
        onTouchEnd = new UnityEvent();
    }

    // Use this for initialization
    void Start()
    {
        // 変数の初期化
        startPosition = Vector2.zero;
        prevPosition = Vector2.zero;
        direction = Vector2.zero;
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
            onTouchBegin.Invoke();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 position = Input.mousePosition;

            if ((position - prevPosition).magnitude > 1.0f) {
                prevPosition = Input.mousePosition;
                direction = prevPosition - startPosition;
                distance = Vector2.Distance(prevPosition, startPosition);
                onTouchMove.Invoke();
            }
            else {
                onTouchStay.Invoke();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            onTouchEnd.Invoke();
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
                    distance = Vector2.Distance(touch.position, startPosition);

                    touched = true;

                    onTouchBegin.Invoke();
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
                    distance = Vector2.Distance(touch.position, startPosition);

                    onTouchMove.Invoke();
                    break;

                // タッチしているが動いていない
                case TouchPhase.Stationary:
                    onTouchStay.Invoke();
                    break;

                // 離れた
                case TouchPhase.Ended:
                    touched = false;

                    onTouchEnd.Invoke();
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
    /// 動いていない時
    /// </summary>
    /// <returns></returns>
    public bool Stay ()
    {
        // デバック
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)) {
            Vector2 position = Input.mousePosition;

            if ((position - prevPosition).magnitude < 1.0f) {
                return true;
            }
        }
        return false;
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary)
                return true;
        }
        return false;
#endif
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
    public Vector2 GetStartPos()
    {
        return startPosition;
    }

    public Vector2 GetPrevPos()
    {
        return prevPosition;
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    public float GetDistance()
    {
        return distance;
    }

}