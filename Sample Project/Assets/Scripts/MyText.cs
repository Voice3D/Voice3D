using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyText : MonoBehaviour {
    private Vector3 m_velocity; // 速度
    public bool m_on_rail;
    public bool m_stop;
    public int m_cylR;
    public int m_id;
    public int m_size;
    public float m_rotSpeed;
    public bool rotation = false;
    	
	// Update is called once per frame
	void Update () {
        //回転する
        if (rotation)
        {            
            transform.parent.Rotate(m_rotSpeed, 0, 0);            
        }
        // 移動する
        if (!(m_stop))
        {
            if (m_on_rail) transform.parent.RotateAround(new Vector3(0, 0, 0), new Vector3(0, -1, 0), 45 * Time.deltaTime);
            else transform.parent.localPosition += m_velocity;
        }
        if (!(m_on_rail) && Vector3.Distance(transform.position, new Vector3(0, transform.position.y, 0)) >= m_cylR)
        {
            m_on_rail = true;
            transform.parent.LookAt(new Vector3(0, transform.parent.position.y, 0));
            transform.parent.Rotate(new Vector3(0, 180, 0));
            TextManager.tm.PosiText(m_id, m_size);
            float ps = (float)TextManager.tm.m_top;
            Vector3 temp = transform.parent.localPosition;
            temp.y = ps;
            transform.parent.localPosition = temp;
        }
    }

    public void Init(int i, float speed, string c, int size)
    {
        var direction = transform.parent.forward;
        m_size = size;
        // 発射角度と速さから速度を求める
        m_velocity = direction * speed;
        transform.localPosition = new Vector3(0, 0.75f, 0);
        transform.localEulerAngles = Vector3.zero;

        m_id = i;
        GetComponent<TextMesh>().text = c;
    }
}
