/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectManager {

	/*
	 * オブジェクトプールの操作をする。プール自体はManagerが持っている。怪しい。
	 */
	public class ObjectPool : MonoBehaviour {

		static ObjectPool instance;

		public static ObjectPool Instance {
			get {
				if (instance == null) {
					instance = (ObjectPool)FindObjectOfType(typeof(ObjectPool));
					//	無ければ生成
					if (instance == null)
						instance = new GameObject("ObjectPool").AddComponent<ObjectPool>();
				}
				return instance;
			}
		}


		#region パブリックメソッド

		/*
		 * オブジェクトをプールから取得。必要に応じて生成
		 */
		public GameObject GetGameObject(GameObject prefab, Vector3 pos, Quaternion rot) {
			// 生成用
			GameObject obj = null;

			//	プレハブのサーチタグを使う
			var tag = prefab.GetComponent<ObjectBase>().SearchTag;


			//	プールにtagが存在しなければ生成
			if (ObjectManager.Instance.list.ContainsKey(tag) == false) {
				//	生成する
				obj = (GameObject)Instantiate(prefab, pos, rot);
				//	プールの子要素にする
				obj.transform.parent = transform;

				ObjectManager.Instance.list.Add(tag, obj);
				return obj;
			}

			List<GameObject> gameObjects = ObjectManager.Instance.list[tag];

			//	使用可能オブジェクト検索ループ
			for (int i = 0; i < gameObjects.Count; i++) {
				obj = gameObjects[i];

				//	非アクティブであれば
				if (obj.activeInHierarchy == false) {
					//	位置の設定
					obj.transform.position = pos;

					//	角度の設定
					obj.transform.rotation = rot;

					//	これから使用する
					obj.SetActive(true);

					return obj;
				}
			}

			//	使用できるものがなかった場合ここまでくる
			//	生成する
			obj = (GameObject)Instantiate(prefab, pos, rot);

			//	プールの子要素にする
			obj.transform.parent = transform;

			//	リストに追加
			gameObjects.Add(obj);

			return obj;
		}


		/*
		 * オブジェクトの非アクティブ化。再利用可能状態へ
		 */
		public void ReleaseGameObject(GameObject obj) {
			//	非アクティブに
			obj.SetActive(false);
		}

		#endregion パブリックメソッド

	}
}
