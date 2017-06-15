/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * デバッグ用FPS表示
 * マスタ版では、排斥すべし。
 */
public class FPSDebug : MonoBehaviour {

	// for fps calculation
	float fps = 0;
	float appTime = 0;

	// Use before Start
	void Awake() {

		Application.targetFrameRate = 60;

	}

	// Update is called once per frame
	void Update() {
		//	1秒に一回更新
		appTime += Time.deltaTime;
		if (appTime >= 1) {
			fps = Mathf.RoundToInt(1f / Time.deltaTime);
			appTime = 0;
		}
		//	描画
		GetComponent<Text>().text = "FPS:" + fps.ToString();
	}
}
