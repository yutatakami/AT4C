/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCreator : MonoBehaviour {

	//	敵プレハブ
	[SerializeField]GameObject batteryPrefab;
	

	// Use this for initialization
	void Start() {
		// 周期的に実行したい場合はコルーチンを使うとよい
		//StartCoroutine(Exec());
		Create();
	}
	

	// 敵を生成
	IEnumerator Exec() {
		while (true) {
			Create();
			yield return new WaitForSeconds(5.0f);
		}
	}

	/*
	 * 単体生成
	 */
	public void Create() {
		//	生成（再利用可能オブジェクトがあればアクティブにする。）
		//ObjectPool.Instance.GetGameObject(enemyPrefab, Vector3.zero, Quaternion.identity);

		//	ランダム（仮）後で消す
		Vector3 pos = new Vector3(Random.Range(-10.0f,10.0f),0.1f, Random.Range(-10.0f, 10.0f));
		ObjectManager.ObjectPool.Instance.GetGameObject(batteryPrefab, pos, Quaternion.identity);
	}

	/*
	 * 指定数生成	いらないかも
	 */
	public void Create(int num) {
		for(int i = 0; i < num; i++) {
			//	生成（再利用可能オブジェクトがあればアクティブにする。）
			ObjectManager.ObjectPool.Instance.GetGameObject(batteryPrefab, Vector3.zero, Quaternion.identity);
		}
	}

}
