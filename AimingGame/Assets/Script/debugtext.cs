using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debugtext : MonoBehaviour {
    [SerializeField]
    Text[] position;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(int num,Vector3 startvec, Vector3 endvec)
    {
        position[num].text = "Line" + (num + 1) + " Position : " + 
            "(" + startvec.x + "," + startvec.y + "," + startvec.z + ")" +
            "(" + endvec.x + "," + endvec.y + "," + endvec.z + ")";
    }
}