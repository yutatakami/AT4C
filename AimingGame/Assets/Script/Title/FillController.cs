/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * タイトル演出　FillAmount操作用
 */
public class FillController : MonoBehaviour {

	//	変化量
	[SerializeField]
	float amountSpeed;

	//	変化開始遅延時間
	[SerializeField]
	float delayTime;

	//	終了フラグ
	public bool endFill { get; set; }

	//	Image
	Image myImage;

	// Use this for initialization
	void Start() {
		myImage = GetComponent<Image>();
		StartCoroutine(UpdateAmount());
		endFill = false;
	}


	IEnumerator UpdateAmount() {

		//	指定秒待機	
		yield return new WaitForSeconds(delayTime);

		//	変化ループ
		while (myImage.fillAmount < 1) {
			myImage.fillAmount += amountSpeed * Time.deltaTime;
			yield return null;
		}

		//	終了
		endFill = true;
	}


}
