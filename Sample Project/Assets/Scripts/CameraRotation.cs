using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {// カメラの向きを制御
    public float rate;
    //public Controller player;
    private float ud_deg=0, rl_deg=0;
	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
        float dx = Input.GetAxis("rotX");
        float dy = Input.GetAxis("rotY");
        
        Vector3 p_posi = Controller.uc.transform.position;
        p_posi.y = 0.4f;
        transform.position = p_posi;
        transform.eulerAngles += new Vector3(-dy*rate, dx*rate, 0);       
    }   
}
