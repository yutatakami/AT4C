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
	public bool shoot = false;

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
			if(shoot)
				Shoot();
		}
	}

	/*
	 * 弾を発射する
	 */
	void Shoot() {
		ObjectManager.ObjectPool.Instance.GetGameObject(bullet, transform.position, transform.rotation);
	}


	/*
	 * あたり判定
	 */
	private void OnCollisionEnter(Collision collision) {

		//
		switch (collision.gameObject.tag) {
			case "First":
				isHitFirst = true;
				//Debug.Log(collision.gameObject.name);
				if (isHitSecond) {
					Destroy(gameObject);
				}
				break;
			case "Second":
				isHitSecond = true;
				//Debug.Log(collision.gameObject.name);
				if (isHitFirst) {
					Destroy(gameObject);
				}
				break;
		}
	}

	/*
	 * 接触が離れた時
	 */
	private void OnCollisionExit(Collision collision) {

		//
		switch (collision.gameObject.tag) {
			case "First":
				isHitFirst = false;
				break;
			case "Second":
				isHitSecond = false;
				break;
		}

	}


	/*
	 * 死亡時
	 */
	private void OnDisable() {

		//	音再生
		if(Sound_Manager.Instance)
			Sound_Manager.Instance.PlaySE("SE003");

	}
}