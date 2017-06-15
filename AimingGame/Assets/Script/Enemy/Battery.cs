/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 固定砲台	向いている方向に撃つのみ
 */
public class Battery : EnemyBase {

	[SerializeField]GameObject bullet;

	//	Hoge
	float interval = 0;

	// Use this for initialization
	void Start () {
		speed = 1.0f;
		faceSpeed = 5.0f;

		//	あたり判定に必要なコンポーネントをコンポーネント
		GetCollision();
	}

	// Update is called once per frame
	void Update() {

		//	弾発射
		interval += Time.deltaTime;
		if (interval >= 5) {
			interval = 0;
			Shoot();
		}
	}

	/*
	 * 弾を発射する
	 */
	void Shoot() {
		ObjectManager.ObjectPool.Instance.GetGameObject(bullet, transform.position, transform.rotation);
	}

}