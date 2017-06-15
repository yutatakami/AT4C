using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ObjectBase {

	[SerializeField]float speed;
	float timer = 0;

	// Use this for initialization
	void Start () {
		searchTag = Search.Tags.Bullet;
	}
	
	// Update is called once per frame
	void Update () {
		//	前進
		FowerdMove();

		//	後で変更必須
		timer += Time.deltaTime;
		if (timer >= 3.0f) {
			timer = 0;
			ObjectManager.ObjectPool.Instance.ReleaseGameObject(gameObject);
		}
	}

	/*
	 * 前進(キャラコンなし)
	 */
	void FowerdMove() {
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
