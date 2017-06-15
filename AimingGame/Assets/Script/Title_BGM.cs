using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_BGM : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Sound_Manager.Instance.PlayBGM("BGM001", true);
        }
	}
}
