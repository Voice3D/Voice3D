using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {
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
        
        //Debug.Log(dx+":"+dy);
        Vector3 p_posi = player.transform.localPosition;
        if (!((-45  > ud_deg && dy < 0) || (ud_deg > 45 && dy > 0)) && dy!=0)
        {
            var angle = -rl_deg;
            //if (transform.localPosition.x > p_posi.x) angle *= -1;
            //Vector3 my_axis = new Vector3(p_posi.z-transform.localPosition.z, 0, -1/(p_posi.x-transform.localPosition.x));
            transform.RotateAround(p_posi, Vector3.up, angle);
            Debug.Log(transform.localPosition+", "+angle);
            transform.RotateAround(p_posi, Vector3.right, dy * rate/3);
            transform.RotateAround(p_posi, Vector3.up, -angle);
            ud_deg += dy * rate;
        }
        transform.RotateAround(p_posi, Vector3.up, dx * rate);
        rl_deg += dx * rate;
        player.transform.localRotation = transform.localRotation;
    }
}
