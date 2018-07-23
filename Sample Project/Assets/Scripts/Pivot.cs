using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pivot : NetworkBehaviour {
    //文字の回転軸を調整するために用いる
    //private static int preId = 0;
    //private static int strCount = 0;

    [ClientRpc]
    public void RpcSetText(int i, char c, int textSize)
    {
        //Debug.Log("char: "+c+", "+isServer);
        var text = transform.GetChild(0).GetComponent<MyText>();
        var direction = transform.forward;
        text.m_size = textSize;
        text.m_velocity = direction * Player.p.m_shotSpeed;
        text.transform.localEulerAngles = Vector3.zero;
        text.transform.localScale *= textSize;
        text.m_id = i;
        text.GetComponent<TextMesh>().text = c.ToString();
        Player.p.AddTexts(text);
    }
}
