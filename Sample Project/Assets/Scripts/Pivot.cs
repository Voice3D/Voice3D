using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pivot : NetworkBehaviour {
    //文字の回転軸を調整するために用いる
    [SyncVar]
    string text;
	// Use this for initialization
	void Start () {
        //Debug.Log("pivot");
        //transform.GetChild(0).GetComponent<TextMesh>().text = text;
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
