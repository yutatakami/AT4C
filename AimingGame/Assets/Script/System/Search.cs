/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 検索につかうタグ置き場
/// </summary>
public class Search : MonoBehaviour {

	//	検索タグ
	public enum Tags {
		None = 0,
		Player,
		Enemy,
		Object,
		Item,
        Line,
		Bullet,
		Max
	}
	
}
