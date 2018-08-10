using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyText : MonoBehaviour {
    public int m_id;
    public int m_size;
    public static float str;
    //public bool start, finish;

    public MyText Init(int i, float speed, string c, int size)//文字生成時の初期化
    {
        var direction = transform.parent.forward;
        m_size = size;
        transform.parent.GetComponent<Pivot>().m_velocity = direction * speed;
        //transform.localPosition = new Vector3(0, 0.75f, 0);
        transform.localEulerAngles = Vector3.zero;
        transform.localScale *= size;

        m_id = i;
        GetComponent<TextMesh>().text = c;
        GetComponent<TextMesh>().color = new Color(0, 0, 0, str);
        return this;
    }
}
