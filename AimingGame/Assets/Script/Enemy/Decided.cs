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
	public bool shoot = false;
	public bool move = false;
	Vector3 startPos;


	// Use this for initialization
	void Start () {
		speed = 1.0f;
		faceSpeed = 5.0f;

		startPos = transform.position;

		//	あたり判定に必要なコンポーネントをコンポーネント
		GetCollision();
	}
	
	// Update is called once per frame
	void Update () {
		//	プレイヤー取得
		var player = ObjectManager.ObjectManager.Instance.SearchPlayer();

		//	移動
		if(move)
			HogeMovePattern(2.0f);	//	移動量
		//	角度
		LockOn(player.transform, 1.0f);
		//	弾発射
		interval += Time.deltaTime;
		if (interval >= 5) {
			interval = 0;
			if(shoot)
				Shoot();
		}
	}


	/*
	 * 仕様固まるまでの仮移動パターン
	 */
	void HogeMovePattern(float r) {
		var pos = transform.position;
		pos.x = startPos.x + r * Mathf.Cos(Time.time);
		transform.position = pos;
	}


	/*
	 * 弾発射
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
		if (Sound_Manager.Instance)
			Sound_Manager.Instance.PlaySE("SE003");

	}
}
