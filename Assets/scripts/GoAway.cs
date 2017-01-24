using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAway : MonoBehaviour {

	void Update () {
        transform.Translate(new Vector3(0.1f, 0));
	}
}
