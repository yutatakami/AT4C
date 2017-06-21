/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 双子の片割れ　ジャマー
 */
public class Jammer : EnemyBase {

	//	LightningBoltEffectの操作用
	DigitalRuby.LightningBolt.LightningBoltScript BoltController;

	//	parent object
	GameObject myParent;

	//	Dead flag
	bool isDead = false;

	//	propaty
	public bool IsDead { get; set; }

	// Use this for initialization
	void Start() {
		//	親の取得
		myParent = transform.parent.gameObject;

		//	ライトニングボルトのコントロール取得
		BoltController = GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
		
		//	エフェクトの曲がる回数をランダムでセット
		BoltController.Generations = Random.Range(4, 8);

		//	あたり判定に必要なコンポーネントをコンポーネント
		GetCollision();

		//	動く物体にぶつかっても動かない
		rb.constraints = RigidbodyConstraints.FreezePosition;
	}


	/*
	 * 非アクティブになった時
	 */
	private void OnDisable() {
		//	死んだ
		isDead = true;

		//	親がいないなら処理しない
		if (myParent == null) return;

		//	親に死んだことを知らせる
		myParent.GetComponent<Twins>().DeadChild = isDead;
		myParent.GetComponent<Twins>().RemoveBolt();

		//	破棄
		Destroy(gameObject);
	}

	/*
	 * 破棄されたとき
	 */
	private void OnDestroy() {

		//	自身をリストから削除
		if (ObjectManager.ObjectManager.Instance == null) return;
		ObjectManager.ObjectManager.Instance.list[SearchTag].Remove(gameObject);

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
}
