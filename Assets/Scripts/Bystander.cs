using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bystander : MonoBehaviour {

	public int Gold;


	void Awake () {
		Gold = Random.Range (10, 21);
	}
}
