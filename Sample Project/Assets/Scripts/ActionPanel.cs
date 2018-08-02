using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour {
    private int choiceLine = 0;
    private int nextLine = 1;
    private int panelNum = 0;
    private float preUd;
    private bool move;
    private MenuPanel mp;
    public int invId;

    // Use this for initialization
    void Start()
    {
        mp = transform.parent.GetComponent<MenuPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        //選択行を変更するか判定
        if (Input.GetButtonDown("Fire1"))
        {
            var temp = TouchpadExmpleright.position.y;
            float ud;
            if (temp > 0.5) ud = 1;
            else if (temp < -0.5) ud = -1;
            else ud = 0;

            Debug.Log("ud: "+ud);

            if (Mathf.Abs(ud) == 1 && ud != preUd) move = true;
            else move = false;
            //if (preUd != ud) Debug.Log(ud);
            preUd = ud;

            //選択行の変更
            if (move && ((choiceLine != 0 && ud == 1) || (choiceLine != 1 && ud == -1)))
            {
                transform.GetChild(0).transform.localPosition += new Vector3(0, 0, mp.heightLine * ud);
                choiceLine -= (int)ud;
            }
            //決定時の操作
            if (ud == 0)
            {
                switch (choiceLine)
                {
                    case 0://取り出す
                        Player.p.PickText(invId);
                        mp.PanelChange(-1);
                        break;
                    case 1://削除
                        Player.p.RemoveInventory(invId);
                        mp.PanelChange(0);
                        break;
                }
            }

        }

            //画面バック
            //if (Input.GetButtonDown("Back")) Debug.Log("back");
            if (Input.GetButtonDown("Back")) mp.PanelChange(0);

        
    }
}
