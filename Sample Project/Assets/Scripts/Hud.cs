using UnityEngine;
using UnityEngine.UI;

// 情報表示用の UI を制御するコンポーネント
public class Hud : MonoBehaviour
{
   
   // public Text m_vText;// レベルのテキスト
 

    // 毎フレーム呼び出される関数
    private void Update()
    {
        
        //m_vText.text = player.m_level.ToString();

        // プレイヤーが非表示ならゲームオーバーと表示する
        //m_gameOverText.SetActive(!player.gameObject.activeSelf);
    }
}