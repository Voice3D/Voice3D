using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyText : MonoBehaviour {
    private Vector3 m_velocity; // 速度
    public bool m_on_rail;
    public bool m_stop;
    public int m_cylR;
    public int m_id;
    	
	// Update is called once per frame
	void Update () {
        // 移動する
        if (!(m_stop))
        {
            if (m_on_rail) transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, -1, 0), 45 * Time.deltaTime);
            else transform.localPosition += m_velocity;
        }
        if (!(m_on_rail) && Vector3.Distance(transform.localPosition, new Vector3(0, transform.localPosition.y, 0)) >= m_cylR)
        {
            m_on_rail = true;
            transform.LookAt(new Vector3(0, transform.localPosition.y, 0));
            transform.Rotate(new Vector3(0, 180, 0));
            TextManager.tm.PosiText(m_id);
            float ps = (float)TextManager.tm.m_top;
            Vector3 temp = transform.localPosition;
            temp.y = ps;
            transform.localPosition = temp;
        }
    }

    public void Init(int i, float speed, string c)
    {
        var direction = transform.forward;

        // 発射角度と速さから速度を求める
        m_velocity = direction * speed;

        m_id = i;
        GetComponent<TextMesh>().text = c;
     
        //Destroy(gameObject, 30);
    }
}
