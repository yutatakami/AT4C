/*
 * Author : IsseiYamada
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComponentEx {

	public static class ComponentExtensions {

		/*
		 * 子の取得
		 */
		public static GameObject[] GetChildren(this Component self) {

			List<GameObject> children = new List<GameObject>();

			foreach (Transform child in self.transform) {
				if (child.gameObject.activeSelf)
					children.Add(child.gameObject);
			}

			return children.ToArray();
		}
	}
}