using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour {
    private int panelNum = 0;
    public int maxLine, minLine, heightLine=1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () { 
	}


    //表示パネルを変更
    public void PanelChange(int to)
    {        
        transform.GetChild(panelNum).gameObject.SetActive(false);
        if (to == -1)
        {
            Player.p.menu = false;
            to = 0;
        }
        transform.GetChild(to).gameObject.SetActive(true);
        panelNum = to;
    }

    
}
