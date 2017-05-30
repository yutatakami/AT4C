using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botton : MonoBehaviour {

    [SerializeField]
    string Scenename;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        Scenemanager.Instance.LoadLevel(Scenename, 0.5f, 0.5f);
    }
}
