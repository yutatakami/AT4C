/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour {

	protected int priority;   //	優先度
	[SerializeField]protected Search.Tags searchTag;	//	検索タグ

	//	アクセサ
	#region Accesser
	public int Priority { get { return priority; } set { priority = value; } }
	public Search.Tags SearchTag { get { return searchTag; } }
	#endregion Accesser

	
	private void Awake() {
		//	継承したオブジェクトは自動でリストへ登録されます
		ObjectManager.ObjectManager.Instance.list.Add(searchTag, gameObject);
	}

}
