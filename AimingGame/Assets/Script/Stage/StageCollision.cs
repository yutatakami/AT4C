/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ステージのコライダーに関するクラス
 */
public class StageCollision : MonoBehaviour {
	


	/*
	 * 接触時
	 */
	private void OnCollisionEnter(Collision collision) {

		switch (collision.gameObject.tag) {
			case "Player":

				//TODO:	ここにプレイヤーが障害物に当たった時の処理（止まるとか？）
				//		を描いてください卍
				//	collision.gameObject.GetComponent</*卍*/>().関数名();	とか？

				break;
		}
	}
}
