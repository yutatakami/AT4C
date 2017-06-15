using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireDestroy : MonoBehaviour {
    int ID; // 
    GameObject playerObj;   // プレイヤーオブジェクト
    CharacterMove charaMove;// キャラ移動
    wireManager wM;         // ワイヤー

	// Use this for initialization
	void Start () {
        playerObj = GameObject.Find("Player").gameObject;
        charaMove = playerObj.GetComponent<CharacterMove>();
        wM = playerObj.GetComponent<wireManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter (Collider other)
    {
        Debug.Log(other.name + "に当たった");
        if (other.transform.tag == "Player") {
            Debug.Log("プレイヤーに当たった");
            wM.WireAllDelete();
            charaMove.InitFlg();
        }
    }

    public void SetWireID(int id)
    {
        ID = id;
    }
}
