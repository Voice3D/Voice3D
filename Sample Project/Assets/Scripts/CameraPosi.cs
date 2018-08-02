using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosi : MonoBehaviour {
    public Transform sight, left, right;
    public static CameraPosi cp;

	// Use this for initialization
	void Start () {
        cp = this;
        /*
        Controller.uc.leftHandObj = left;
        Controller.uc.rightHandObj = right;
        Controller.uc.lookAtObj = sight;
        */
    }
	
	// Update is called once per frame
	void Update () {
        var pos = Controller.uc.transform.position;
        //transform.rotation = Controller.uc.transform.rotation;
        pos.y = 0;
        transform.position = pos;
	}

    public void setObj() {
        Controller.uc.leftHandObj = left;
        Controller.uc.rightHandObj = right;
        Controller.uc.lookAtObj = sight;
    }
}
