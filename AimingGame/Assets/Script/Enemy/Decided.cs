/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 動いて（パターン）、プレイヤーを狙い弾を撃つ
 */
public class Decided : EnemyBase {

	//	Bullet object
	[SerializeField]GameObject bullet;

	//	Hoge
	float interval = 0;

	// Use this for initialization
	void Start () {
		speed = 1.0f;
		faceSpeed = 5.0f;

		//	キャラコン取得
		GetCharacterController();
	}
	
	// Update is called once per frame
	void Update () {
		//	プレイヤー取得
		var player = ObjectManager.ObjectManager.Instance.SearchPlayer();

		//	移動
		HogeMovePattern(5.0f);
		//	角度
		LockOn(player.transform, 1.0f);
		//	弾発射
		interval += Time.deltaTime;
		if (interval >= 5) {
			interval = 0;
			Shoot();
		}
	}


	/*
	 * 仕様固まるまでの仮移動パターン
	 */
	void HogeMovePattern(float r) {
		var pos = transform.position;
		pos.x = r * Mathf.Cos(Time.time);
		transform.position = pos;
	}


	/*
	 * 弾発射
	 */
	void Shoot() {
		ObjectManager.ObjectPool.Instance.GetGameObject(bullet, transform.position, transform.rotation);
	}
}
