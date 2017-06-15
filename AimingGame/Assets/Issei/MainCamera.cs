/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
	
	GameObject player;
	Vector3 offset;     //	オフセット値


	//	イベントメソッド
	#region Event Method

	// Use this for initialization
	void Start() {
		//	オフセット値の設定
		offset = new Vector3(0, 10, -15);

		//	プレイヤー取得
		player = ObjectManager.ObjectManager.Instance.SearchPlayer();
	}


	// Update is called once per frame
	void Update() {
		//	カメラの位置を固定、なめらかに
		Vector3 new_pos = player.transform.position + offset;
		transform.localPosition = Vector3.Lerp(transform.position, new_pos, 5.0f * Time.deltaTime);
	}


	#endregion Event Method

	
}
