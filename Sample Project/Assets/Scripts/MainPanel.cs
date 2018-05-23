using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour {
    private int choiceLine;
    private int nextLine = 8;
    private int panelNum = 0;
    private float preUd = 0;
    private bool move = false;
    private MenuPanel mp;

    // Use this for initialization
    void Start () {
        mp = transform.parent.GetComponent<MenuPanel>();
        choiceLine = mp.minLine;
    }

    // Update is called once per frame
    void Update()
    {
        //選択行を変更するか判定
        var ud = Input.GetAxis("UpDn");
        if (Mathf.Abs(ud) == 1 && ud != preUd) move = true;
        else move = false;
        if (preUd != ud) Debug.Log(ud);
        preUd = ud;

        //選択行の変更
        if (move && ((choiceLine != mp.minLine && ud == 1) || (choiceLine != nextLine-1 && ud == -1)))
        {            
            transform.GetChild(0).transform.localPosition += new Vector3(0, 0, mp.heightLine * ud);
            choiceLine -= (int)ud;
        }

        //決定時の操作
        if (Input.GetButtonDown("Fire1"))
        {
            mp.PanelChange(1);
            mp.transform.GetChild(1).GetChild(1).transform.GetComponent<TextMesh>().text =
                transform.GetChild(choiceLine+2).GetComponent<TextMesh>().text;
            mp.transform.GetChild(1).GetComponent<ActionPanel>().invId = choiceLine;
        }

    }

    //インベントリの追加
    public void SetInventory(string str)
    {
        transform.GetChild(nextLine).GetComponent<TextMesh>().text = str;
        nextLine++;
    }

    //インベントリの削除
    public void PickInventory(int id)
    {
        int i;
        for (i = id; i < nextLine-2; i++)
        {
            transform.GetChild(i+2).GetComponent<TextMesh>().text = transform.GetChild(i + 3).GetComponent<TextMesh>().text;
        }
        transform.GetChild(nextLine + 1).GetComponent<TextMesh>().text = "";
        nextLine--;
    }
}
