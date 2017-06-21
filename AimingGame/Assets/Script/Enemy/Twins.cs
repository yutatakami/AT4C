/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentEx;

/*
 * 双子の親　ジャマーの親
 */
public class Twins : EnemyBase {
	//	member
	float distance;	//	二機の距離格納用
	Vector3 size;
	public GameObject[] children = new GameObject[2];	//	←パブリックである必要はない

	//	propaty 
	public bool DeadChild{ get; set; }


	private void Start() {

		float hoge = 10.0f;

		//	子の双子を配列で取得
		children = ComponentEx.ComponentExtensions.GetChildren(this);

		//	二人の距離をとる(ハート)
		SetChildPosition(hoge);

		//	2機間にダメージ判定を展開
		CreateDamageBox(hoge * 2);
	}


	/*
	 * 2機間のダメージ判定ボックス生成
	 */
	void CreateDamageBox(float boxsize) {
		gameObject.AddComponent<BoxCollider>();
		size = gameObject.GetComponent<BoxCollider>().size;
		size.x = boxsize;
		gameObject.GetComponent<BoxCollider>().size = size;

		//	通り抜ける
		gameObject.GetComponent<BoxCollider>().isTrigger = true;
	}


	/*
	 * ボルトエフェクトの解除
	 */
	public void RemoveBolt() {

		foreach (Transform child in transform) {
			//	死んだ子は飛ばす
			if (child.GetComponent<Jammer>().IsDead) continue;
			//	残された子からボルトエフェクトをはがす。
			Destroy(child.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>());
			Destroy(child.GetComponent<LineRenderer>());
		}
		//	ボルトのあたり判定を削除
		Destroy(gameObject.GetComponent<BoxCollider>());
	}


	/*
	 * 子の配置（親からのローカル座標指定）
	 */
	void SetChildPosition(float distance) {
		children[0].transform.localPosition += new Vector3(distance, 0, 0);
		children[1].transform.localPosition += new Vector3(-distance, 0, 0);
	}


	/*
	 * 非アクティブになった時
	 */
	private void OnDisable() {

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
	 * ダメージボックスの判定
	 */
	private void OnTriggerEnter(Collider other) {

		switch (other.gameObject.tag) {
			case "Player":
				//TODO: プレイヤーのストックを減らす的な処理
				//		三回ルールです。
				break;
		}
	}
}
