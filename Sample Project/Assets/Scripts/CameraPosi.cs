using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosi : MonoBehaviour {
    public Transform sight, left, right;
    public SteamVR_Render steamvr;
    public GameObject mc;
    public static CameraPosi cp;
    private float rate = 0.001f, x=0, y=0;
    //private int counter=100;

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
        //counter--;
        if (!steamvr.enabled) {
            Controller.uc.vr = false;
            mc.SetActive(true);
            gameObject.SetActive(false);
            var rx = Input.GetAxis("rotX");
            var ry = Input.GetAxis("rotY");
            Debug.Log("rxy: "+x +", "+y);
            //transform.eulerAngles += new Vector3(0, 90f, 0);
            x += rx;
            y += ry;
            transform.eulerAngles = new Vector3(y*rate, x*rate, 0);
            //transform.Rotate(Vector3.up, -rx*rate);
            //transform.Rotate(Vector3.right, ry*rate);
            pos.y = 0.4f;
            //counter = 100;
        }
        transform.position = pos;
	}

    public void setObj() {
        Controller.uc.leftHandObj = left;
        Controller.uc.rightHandObj = right;
        Controller.uc.lookAtObj = sight;
    }
}
