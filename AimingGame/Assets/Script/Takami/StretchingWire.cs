using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchingWire : MonoBehaviour {
    float LineLength;   // 線の長さ
    float Angle;        // 始点から終点までの角度

    [SerializeField]
    float STRETCHSPEED; // 伸びる速度

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ワイヤーとなるオブジェクトを徐々に伸ばしていくコルーチン
    /// </summary>
    /// <param name="cylinder">ワイヤーとなるゲームオブジェクト</param>
    /// <param name="Position">始点</param>
    /// <param name="Length">長さ</param>
    /// <param name="Angle">角度</param>
    /// <returns></returns>
    public Coroutine Stretch(GameObject cylinder, Vector3 Position, float Length, float Angle)
    {
        return StartCoroutine(StretchCoroutine(cylinder, Position, Length, Angle));
    }


    // 徐々に伸ばす
    IEnumerator StretchCoroutine (GameObject cylinder, Vector3 Position, float Length, float Angle)
    {
        float NowLength = 0.0f;
        var endFrame = new WaitForEndOfFrame();

        while(NowLength <= Length / 2) {
            NowLength += STRETCHSPEED;
            cylinder.transform.localScale = new Vector3(cylinder.transform.localScale.x,
                cylinder.transform.localScale.y, NowLength);
            cylinder.transform.position = new Vector3(
                Position.x + NowLength * Mathf.Cos(Angle),
                Position.y,
                Position.z + NowLength * Mathf.Sin(Angle));

            yield return endFrame;
        }

        //cylinder.transform.localScale = new Vector3(cylinder.transform.localScale.x,
        //    cylinder.transform.localScale.y, Length);
        cylinder.transform.position = new Vector3(
            Position.x + Length / 2 * Mathf.Cos(Angle),
            Position.y,
            Position.z + Length / 2 * Mathf.Sin(Angle));
    }
}
