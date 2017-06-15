using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallcollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionStay(Collision Info)
    {
        Info.gameObject.transform.position = gameObject.transform.position;
    }
}
