using UnityEngine;
using UnityEngine.UI;

// 情報表示用の UI を制御するコンポーネント
public class Hud : MonoBehaviour
{

    //public Text m_vText;// レベルのテキスト
    public Text m_Mode;
    public Text m_Trigger;
    public Player m_player;
 

    // 毎フレーム呼び出される関数
    private void Update()
    {
        m_Mode.text = m_player.GetMode().ToString();
        if (Input.GetAxis("Ltrigger") == 1) m_Trigger.text = "on";
        else m_Trigger.text = "off";
    }
}