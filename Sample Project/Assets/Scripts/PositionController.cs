using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionContrpller : MonoBehaviour {

    public Transform Camera;
    public Transform Unitychan;
    public Transform Head;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Unitychan.position = Camera.position - (Head.position - Unitychan.position);
    }
}
