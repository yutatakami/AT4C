/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeco : ObjectBase {

	public GameObject Prefab;

	// Use this for initialization
	void Start () {
		//ObjectManager.Instance.list.Add(Search.Tags.Player, ObjectPool.Instance.GetGameObject(gameObject, Vector3.zero, Quaternion.identity));
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			//Debug.Log(ObjectManager.Instance.SearchGameObject(Search.Tags.Player, 0).name);
			ObjectManager.ObjectPool.Instance.GetGameObject(gameObject, Vector3.zero, Quaternion.identity);
		}

		if (Input.GetKeyDown(KeyCode.Alpha9)) {

			for (int p = 0; p < ObjectManager.ObjectManager.Instance.list.Count; p++) {
				Debug.Log("：" + ObjectManager.ObjectManager.Instance.list[searchTag][p].name);
			}
		}

	}

}
