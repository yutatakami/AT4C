/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectManager {

	public class ObjectManager : MonoBehaviour {

		public MultiDictionary<Search.Tags, GameObject> list;   //	オブジェクト格納リスト

		//	コンストラクタ
		#region Constructor

		ObjectManager() {
			if (list == null)
				list = new MultiDictionary<Search.Tags, GameObject>();
		}

		#endregion Constructor

		//	シングルトン
		#region Singleton

		static ObjectManager instance;
		public static ObjectManager Instance {
			get {
				if (instance == null) {
					instance = (ObjectManager)FindObjectOfType(typeof(ObjectManager));

					if (instance == null) {
						Debug.LogError(typeof(ObjectManager) + "is nothing");
					}
				}

				return instance;
			}
		}

		#endregion Singleton

		//	イベントメソッド
		#region Event Method

		void Awake() {
			if (this != Instance) {
				Destroy(this.gameObject);
				return;
			}
		}

		#endregion Event Method

		//	パブリックメソッド
		#region Public Method


		/// <summary>
		/// プレイヤー検索
		/// </summary>
		/// <returns></returns>
		public GameObject SearchPlayer() {

			//	プレイヤータグ自体がリストに存在しない
			if (list.ContainsKey(Search.Tags.Player) == false) return null;

			//	プレイヤーが1つも存在しない
			if (list[Search.Tags.Player].Count <= 0) return null;

			//	プレイヤーオブジェクトを返却
			return list[Search.Tags.Player][0];
		}



		/// <summary>
		/// 指定タグのオブジェクトをリストで返す
		/// </summary>
		/// <param name="search_tag"></param>
		/// <returns></returns>
		public List<GameObject> ByTag(Search.Tags search_tag) {
			if (list.ContainsKey(search_tag) == false) return null;

			return list[search_tag];
		}



		/// <summary>
		/// オブジェクト検索返却
		/// </summary>
		/// <param name="search_tag">検索用タグ</param>
		/// <param name="number">リストに何番目に登録されているかの指定</param>
		/// <returns></returns>
		public GameObject SearchGameObject(Search.Tags search_tag, int number) {

			//	検索対象タグ自体がリストに存在しない
			if (list.ContainsKey(search_tag) == false) return null;

			//	検索対象が1つも存在しない
			if (list[search_tag].Count <= 0) return null;

			//	検索対象を返却
			return list[search_tag][number];
		}
		#endregion Public Method

	}

}
