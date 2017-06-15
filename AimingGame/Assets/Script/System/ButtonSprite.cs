/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *	uGUIのボタンを任意のSpriteに変更する。
 */
public class ButtonSprite : MonoBehaviour {

	//	Image
	Image buttonImage;

	//	2DSprite
	public Sprite mySprite;

	// Use this for initialization
	void Start () {
		buttonImage = GetComponent<Image>();
		buttonImage.sprite = mySprite;
	}
	
}
