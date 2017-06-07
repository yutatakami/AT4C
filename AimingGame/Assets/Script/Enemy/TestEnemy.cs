using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase {

	public bool move = false;   //	ほげ
	Vector3 newAngle;
	

	// Use this for initialization
	void Start () {

		GetCharacterController();

		vital = 100.0f;
		speed = 1.0f;

		//
		newAngle.y = 90;
	}
	
	// Update is called once per frame
	void Update () {

		//	移動可能
		//if (move) {
		//	var move_value = Velocity(transform.position, WayPointManager.Instance.POINT2.transform.position);
		//	transform.LookAt(WayPointManager.Instance.POINT2.transform);
		//	controller.Move(move_value * Time.deltaTime);
		//}
		//else {
		//	Slerp();
		//}
	}

	//
	void Slerp() {

		//	角度差
		//newAngle.y = GetAim(transform.position, WayPointManager.Instance.POINT2.transform.position);

		//if (Mathf.DeltaAngle(transform.eulerAngles.y, newAngle.y) < -0.1f) {
		//	transform.Rotate(transform.up, 2f);
		//}
		//if (Mathf.DeltaAngle(transform.eulerAngles.y, newAngle.y) > 0.1f) {
		//	transform.Rotate(transform.up, -2f);
		//}
	}

	//
	void Forward() {
		var pos = transform.position;
		pos += transform.forward * speed * Time.deltaTime;
		transform.position = pos;
	}

	// p2からp1への角度を求める
	// @param p1 自分の座標
	// @param p2 相手の座標
	// @return 2点の角度(Degree)
	public float GetAim(Vector3 p1, Vector3 p2) {
		float dx = p2.x - p1.x;
		float dz = p2.z - p1.z;
		float rad = Mathf.Atan2(dz, dx);
		return rad * Mathf.Rad2Deg;
	}

	

}
