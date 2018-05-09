using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private Vector3 m_velocity; // 速度
    public bool m_on_rail;
    public int m_cylR;
    public int m_id;

    // Update is called once per frame
    void Update()
    {
        // 移動する
        if (!(m_on_rail)) transform.localPosition += m_velocity;
        else transform.localPosition -= m_velocity;
        if (!(m_on_rail) && Vector3.Distance(transform.localPosition, new Vector3(0, transform.localPosition.y, 0)) >= m_cylR)
        {
            m_on_rail = true;   
        }
    }

    public void Init(float speed, Vector3 direction)
    {

        // 発射角度と速さから速度を求める
        m_velocity = direction * speed;

        Destroy(gameObject, 30);
    }

    public void OnCollisionEnter(Collision col) {
        Debug.Log("catch");
        if (col.gameObject.tag == "Text") {
            Debug.Log("check");
            col.gameObject.transform.parent = transform;
        }
    }
}
