/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : ObjectBase {

	// CharacterController
	protected CharacterController controller = null;

	//	BoxCollider
	protected BoxCollider boxCollider = null;

	//	Rigidbody
	protected Rigidbody rb = null;
	
	//	Enemy status
	protected float vital;    //体力
	protected float speed;    //移動速度
	protected float faceSpeed;  //	振り向きの速度
	protected bool isHitFirst = false;	//	一本目のワイヤーにあたった
	protected bool isHitSecond = false;	//	二本目のワイヤーにあたった

	// Property
	public float Vital {
		get { return vital; }
		set { vital = value; }
	}

	public float Speed {
		get { return speed; }
		set { speed = value; }
	}

	public float FaceSpeed {
		get { return faceSpeed; }
		set { faceSpeed = value; }
	}
	

	//	Method

	/*
	 * あたり判定系貼り付け
	 */
	 public void GetCollision() {
		//	BoxCollider取得
		boxCollider = GetComponent<BoxCollider>();
		if(boxCollider == null) {
			//	無ければ付ける
			gameObject.AddComponent<BoxCollider>();
			boxCollider = GetComponent<BoxCollider>();
		}
		//	リファクタリング対象-------------------------------------------------------------------------------
		var size = boxCollider.size;
		size.y = 5;
		boxCollider.size = size;
		size = boxCollider.center;
		size.y = 2.5f;
		boxCollider.center = size;
		//	リファクタリング対象--------------------------------------------------------------------------------

		//	Rigidbody取得
		rb = GetComponent<Rigidbody>();
		if(rb == null) {
			//	無ければ付ける
			gameObject.AddComponent<Rigidbody>();
			rb = GetComponent<Rigidbody>();
		}
		rb.mass = 0.1f; //	これやばいああああああああああああああああああああああああああああああああああああああああああああああ
		rb.drag = 5;

		//	constrainsの設定。
		rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

		//	
		rb.useGravity = false;
	}
	

	/*
	 * CharacterControllerの取得
	 */
	public void GetCharacterController() {
		//	CharacterController取得
		controller = GetComponent<CharacterController>();
		if (controller == null) {
			//	無ければ付ける
			gameObject.AddComponent<CharacterController>();
			controller = GetComponent<CharacterController>();
		}
	}
	
	/*
	 * 前進
	 */
	public void FowerdMove() {
		controller.Move(transform.forward * Time.deltaTime);
	}


	/*
	 * 対象の方向を向く
	 * target : 向きたい対象
	 * face_speed : 向く速度
	 */
	public bool LockOn(Transform target, float face_speed) {

		var relativePos = target.position - transform.position;
		var rotation = Quaternion.LookRotation(relativePos);

		//	最短で向けるはず
		if (Mathf.DeltaAngle(transform.eulerAngles.y, rotation.eulerAngles.y) < -1.0f) {
			transform.Rotate(transform.up, -face_speed);
			return true;
		}
		if (Mathf.DeltaAngle(transform.eulerAngles.y, rotation.eulerAngles.y) >  1.0f) {
			transform.Rotate(transform.up, face_speed);
			return true;
		}

		return false;
	}

	
	/*
	 * 速度ベクトルを返す
	 * start : 自身のposition
	 * end : 対象のposition
	 */
	public Vector3 Velocity(Vector3 start, Vector3 end) {
		//	差を求める
		var diffX = end.x - start.x;
		var diffY = end.z - start.z;
		//	ベクトルを計算
		var direction = Mathf.Sqrt(diffX * diffX + diffY * diffY);
		Vector3 velocity = Vector3.zero;
		//	ゼロ除算を回避
		if (Mathf.Round(direction) == 0) direction = 1;
		velocity.x = diffX / direction;
		velocity.z = diffY / direction;

		return velocity;
	}




}
