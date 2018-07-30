using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {// カメラの向きを制御
    public float rate;
    public Controller player;
    private float ud_deg=0, rl_deg=0;
	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
        float dx = Input.GetAxisRaw("Camera_x");
        float dy = Input.GetAxisRaw("Camera_y");
        
        Vector3 p_posi = player.transform.localPosition;

        if (!((-45  > ud_deg && dy < 0) || (ud_deg > 45 && dy > 0)) && dy!=0)
        {
            var angle = -rl_deg;
            transform.RotateAround(p_posi, Vector3.up, angle);
            transform.RotateAround(p_posi, Vector3.right, dy * rate/3);
            transform.RotateAround(p_posi, Vector3.up, -angle);
            ud_deg += dy * rate/3;
        }
        transform.RotateAround(p_posi, Vector3.up, dx * rate);
        if (Input.GetButtonDown("Lbutton")) {
            
            var angle2 = -rl_deg;
            //transform.RotateAround(p_posi, Vector3.up, angle2);
            transform.position = player.transform.position + new Vector3(0, 1.5f, -4f);
            transform.localEulerAngles = new Vector3(-10f, 0, 0);
            //transform.RotateAround(p_posi, Vector3.right, -ud_deg);
            transform.RotateAround(p_posi, Vector3.up, -angle2);
            
            ud_deg = 0;
            
            
        }

        player.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        rl_deg += dx * rate;
        player.transform.GetChild(0).localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        
    }   
}
