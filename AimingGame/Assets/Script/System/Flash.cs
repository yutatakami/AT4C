using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {

	Image button;
	float volume = 0;

	// Use this for initialization
	void Start () {
		button = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		volume += 5 * Time.deltaTime;
		var color = button.color;
		color.a = Mathf.Sin(volume) + 1.0f;
		button.color = color;
	}
}
