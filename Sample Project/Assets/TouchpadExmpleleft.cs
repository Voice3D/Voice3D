using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadExmpleleft : MonoBehaviour {
    public SteamVR_TrackedObject trackedObject { get; set; }
    public SteamVR_Controller.Device device { get; set; }
    public static Vector2 position;

    public static bool trigger_flag;

	// Use this for initialization
	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);
        trigger_flag = false;
    }
	
	// Update is called once per frame
	void Update () {
        position = device.GetAxis();
        Debug.Log("x: " + position.x + " y: " + position.y);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            trigger_flag = true;
        }
        else
        {
            trigger_flag = false;
        }
    }
}
