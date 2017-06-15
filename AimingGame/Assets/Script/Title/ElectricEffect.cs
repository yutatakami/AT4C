/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * タイトル画面でのスプライトエフェクト
 */
public class ElectricEffect : MonoBehaviour {

	Image electricEffect;

	/*
	 * 透明にしておく
	 */
	void Awake() {
		electricEffect = GetComponent<Image>();
		var color = electricEffect.color;
		color.a = 0;
		electricEffect.color = color;
	}

	/*
	 * フェードイン処理呼ぶ
	 */
	// Use this for initialization
	void Start () {
		StartCoroutine("FadeIn");
	}

	/*
	 * １秒待機し、フェードイン
	 */
	IEnumerator FadeIn() {

		yield return new WaitForSeconds(1.0f);	//	一秒待機し、下のアルファ処理へ

		//	アルファループ
		for (float alpha = 0f; alpha <= 1.0f; alpha += Time.deltaTime) {
			var color = electricEffect.color;
			color.a += alpha;
			electricEffect.color = color;
			yield return null;
		}
	}
}
