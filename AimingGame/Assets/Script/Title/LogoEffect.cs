/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoEffect : MonoBehaviour {

	RectTransform rectTransform;
	Image myImage;

	[SerializeField]float scaleValue;	//	拡大量
	[SerializeField]float alphaValue;   //	アルファ値減衰量

	[SerializeField]
	GameObject virticalObj;
	[SerializeField]
	GameObject underbarObj;


	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();
		myImage = GetComponent<Image>();
		myImage.color = new Color(1,1,1,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(underbarObj.GetComponent<FillController>().endFill && virticalObj.GetComponent<FillController>().endFill) {
			myImage.color = new Color(1, 1, 1, 0.5f);
			StartCoroutine(PlayEffect());
		}
	}


	/*
	 * 拡大しながら透けて消えていく
	 */
	IEnumerator PlayEffect() {

		//	演出ループ
		while (myImage.color.a > 0) {
			ScaleUp();	//	拡大
			FadeOut();	//	透過
			yield return null;
		}

		//	演出終了
		gameObject.SetActive(false);
	}


	/*
	 * 
	 */
	void ScaleUp() {

		if (rectTransform.localScale.x < 1.5f) {//	マジックナンバーごめん
			rectTransform.localPosition += new Vector3(0, 4 * Time.deltaTime, 0);	//	ごめんここで移動入れちゃった
			rectTransform.localScale += new Vector3(scaleValue * Time.deltaTime, scaleValue * Time.deltaTime, 0);
		}
	}

	
	/*
	 * 
	 */
	void FadeOut() {
		var color = myImage.color;
		color.a -= alphaValue * Time.deltaTime;
		myImage.color = color;
	}
}
