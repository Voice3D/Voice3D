using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pivot : NetworkBehaviour {
    //文字の回転軸を調整するために用いる
    //private static int preId = 0;
    //private static int strCount = 0;
    public Vector3 m_velocity; // 速度
    public bool m_on_rail;
    [SyncVar]
    public bool m_stop;
    public int m_cylR;
    public float m_rotSpeed;
    [SyncVar]
    public bool rotation = false;
    private MyText child;

    private void Update()
    {
        //回転する
        if (isServer)
        {
            if (rotation)
            {
                transform.Rotate(m_rotSpeed, 0, 0);
            }
            // 移動する
            if (!(m_stop))
            {
                if (m_on_rail) transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, -1, 0), 45 * Time.deltaTime);
                else transform.position += m_velocity;
            }
            if (!(m_on_rail) && Vector3.Distance(transform.position, new Vector3(0, transform.position.y, 0)) >= m_cylR)
            {
                m_on_rail = true;
                transform.LookAt(new Vector3(0, transform.position.y, 0));
                transform.Rotate(new Vector3(0, 180, 0));
                var h = TextManager.tm.PosiText(child.m_id, child.m_size, this);
                float ps = (float)TextManager.tm.m_top - TextManager.tm.heightPL * (child.m_size - 1 + h);
                Vector3 temp = transform.position;
                temp.y = ps;
                transform.position = temp;
            }
        }
    }

    [ClientRpc]
    public void RpcSetText(int i, char c, int textSize)
    {
        //Debug.Log("char: "+c+", "+isServer);
        child = transform.GetChild(0).GetComponent<MyText>();
        var direction = transform.forward;
        child.m_size = textSize;
        m_velocity = direction * Player.p.m_shotSpeed;
        child.transform.localEulerAngles = Vector3.zero;
        child.transform.localScale *= textSize;
        child.m_id = i;
        child.GetComponent<TextMesh>().text = c.ToString();
        //Player.p.AddTexts(text);
    }

    [Command]
    public void CmdSa(bool flag)
    {
        RpcSa(flag);       
    }

    [ClientRpc]
    public void RpcSa(bool flag)
    {
        gameObject.SetActive(flag);
    }
}
