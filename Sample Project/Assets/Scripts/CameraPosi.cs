using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosi : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var pos = Player.p.transform.parent.transform.position;
        pos.y = 0;
        transform.position = pos;
	}
}
