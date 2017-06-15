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

	[SerializeField]bool canShoot;

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
	void Update() {

		if (!canShoot) return;
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