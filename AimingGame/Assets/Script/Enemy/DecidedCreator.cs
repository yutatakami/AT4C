/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 仕様書曰くノーマル型のクリエイター
 */
public class DecidedCreator : MonoBehaviour {

	//	敵プレハブ
	[SerializeField]GameObject decidedPrefab;
	// アクティブの最大数
	//[SerializeField]int maxEnemy = 2;


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
		Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f), 0.1f, Random.Range(-10.0f, 10.0f));
		ObjectManager.ObjectPool.Instance.GetGameObject(decidedPrefab, pos, Quaternion.identity);
	}

	/*
	 * 指定数生成	いらないかも
	 */
	public void Create(int num) {
		for (int i = 0; i < num; i++) {
			//	生成（再利用可能オブジェクトがあればアクティブにする。）
			ObjectManager.ObjectPool.Instance.GetGameObject(decidedPrefab, Vector3.zero, Quaternion.identity);
		}
	}

}
