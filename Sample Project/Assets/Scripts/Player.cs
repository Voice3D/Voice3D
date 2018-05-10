using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// プレイヤーを制御するコンポーネント
public class Player : MonoBehaviour
{
    public static Player p;
    public Text m_textPrefab; // 弾のプレハブ
    public Arm m_armPrefab;
    public float m_shotSpeed; // 弾の移動の速さ
    public float m_shotTimer;
    public float m_shotInterval;

    public int ScreenWidth;
    public int ScreenHeight;

    private bool waiter = false;
    private string waitS;
    private int counter;
    private int strCount=0;
    private Vector3 shot_pos;
    private Quaternion shot_rot;
    private List<Text> texts;

    public LayerMask mask;

    // ゲーム開始時に呼び出される関数
    private void Awake()
    {
        // PC向けビルドだったらサイズ変更
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Screen.SetResolution(ScreenWidth, ScreenHeight, false);
        }

        // 他のクラスからプレイヤーを参照できるように
        // static 変数にインスタンス情報を格納する
        p = this;
        
    }

    // 毎フレーム呼び出される関数
    private void Update()
    {
        
               
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * 30, Color.red, 3.0f);
        //Debug.Log(Physics.Raycast(ray, out hit, 30));
        if (Physics.Raycast(ray, out hit, 30))
        {
            //Debug.Log(hit.point.z);
            transform.LookAt(hit.point);
            //Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
        }
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) ThrowArm(m_shotSpeed);

        if (waiter)
        {

            // 弾の発射タイミングを管理するタイマーを更新する
            m_shotTimer += Time.deltaTime;

            // まだ弾の発射タイミングではない場合は、ここで処理を終える
            if (m_shotTimer < m_shotInterval) return;

            // 弾の発射タイミングを管理するタイマーをリセットする
            m_shotTimer = 0;


            // 弾を発射する
            //Debug.Log(counter);
            ShootText(strCount, m_shotSpeed, waitS[waitS.Length-counter]);
            counter--;
            if (counter == 0)
            {
                waiter = false;
                TextManager.strs.Add(strCount, texts);
                strCount = (strCount + 1) % 10;  
            }
        }
    }

    public void ThrowText(string s) {
        waitS = s;
        counter = waitS.Length;
        if(counter > 0) waiter = true;  
        m_shotTimer = m_shotInterval;
        shot_pos = transform.position;
        shot_rot = transform.rotation;
        texts = new List<Text>();
    }

    public void ThrowArm(float speed) {
        var pos = transform.position; // プレイヤーの位置
        var rot = transform.rotation; // プレイヤーの向き
        var arm = Instantiate(m_armPrefab, pos, rot);
        
            // 弾を発射する方向と速さを設定する
            arm.Init(speed, transform.forward);
       


    }

    // 弾を発射する関数
    private void ShootText(int i, float speed, char c)
    {
        var pos = shot_pos; // プレイヤーの位置
        var rot = shot_rot; // プレイヤーの向き

                    // 発射する弾を生成する
            // 発射する弾を生成する
        var text = Instantiate(m_textPrefab, pos, rot);
        text.Init(i, speed, c.ToString());
        texts.Add(text);
        
    }  
}