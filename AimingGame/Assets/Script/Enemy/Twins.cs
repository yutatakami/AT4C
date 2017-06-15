/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 双子の親　ジャマーの親
 */
public class Twins : EnemyBase {
	//	子の死亡
	bool deadChild = false;
	
	//	propaty 
	public bool DeadChild{ get; set; }

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
	}


	/*
	 * 非アクティブになった時
	 */
	private void OnDisable() {

		//	自身をリストから削除
		ObjectManager.ObjectManager.Instance.list[SearchTag].Remove(gameObject);

	}

	/*
	 * 破棄されたとき
	 */
	private void OnDestroy() {

		//	自身をリストから削除
		ObjectManager.ObjectManager.Instance.list[SearchTag].Remove(gameObject);

	}
}
