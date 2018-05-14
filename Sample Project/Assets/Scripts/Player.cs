﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// プレイヤーを制御するコンポーネント
public class Player : MonoBehaviour
{
    public static Player p;
    public MyText m_textPrefab; // 弾のプレハブ
    public Arm m_armPrefab;
    public VoiceRecognition m_vr;
    public Pivot m_pivotPrefab;
    public float m_shotSpeed; // 弾の移動の速さ
    public float m_shotTimer;
    public float m_shotInterval;
    public int MaxLength;
    public int maxStrs;

    public int ScreenWidth;
    public int ScreenHeight;

    private bool waiter = false;
    private string waitS;
    private int counter;
    private int strCount=0;
    private Vector3 shot_pos;
    private Quaternion shot_rot;
    private List<MyText> texts;
    private bool triggerOn = false;
    private int m_mode = 0;
    private int textSize;
    private string[] keywords = { "ストップ", "スタート" };

    public LayerMask mask;

    // ゲーム開始時に呼び出される関数
    private void Awake()
    {
        /*
        // PC向けビルドだったらサイズ変更
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Screen.SetResolution(ScreenWidth, ScreenHeight, false);
        }
        */
        // 他のクラスからプレイヤーを参照できるように
        // static 変数にインスタンス情報を格納する
        p = this;
        
    }

    // 毎フレーム呼び出される関数
    private void Update()
    {
        
        //照準の向き、及びアーム発射制御
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30))
        {
            transform.LookAt(hit.point);
        }
        //if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) ThrowArm(m_shotSpeed);

        //音声認識の開始、停止、モードの制御
        if (!triggerOn && Input.GetAxis("Ltrigger") == 1)
        {
            triggerOn = true;
            m_vr.StartRecognition();
        }
        if (triggerOn && Input.GetAxis("Ltrigger") == 0)
        {
            triggerOn = false;
            m_vr.StopRecognition();
        }
        if (!triggerOn && Input.GetButtonDown("change")) m_mode = (m_mode + 1) % keywords.Length;

        //テキスト発射制御
        if (waiter)
        {
            //テキスト発射間隔の調整
            m_shotTimer += Time.deltaTime;
            if (m_shotTimer < m_shotInterval) return;
            m_shotTimer = 0;


            // テキストを発射する
            ShootText(strCount, m_shotSpeed, waitS[waitS.Length-counter]);
            counter--;
            if (counter == 0)
            {
                waiter = false;
                TextManager.strs.Add(strCount, texts);
                strCount = (strCount + 1) % maxStrs;  
            }
        }
    }

    public void ThrowText(string s)//テキスト発射要請
    {
        if (m_mode == 1)
        {
            OpeRecognize(s);
            return;
        }
        waitS = s;
        counter = waitS.Length;
        textSize = 1;
        if (counter > MaxLength)
        {
            Debug.Log("erorr:Too Long");
            return;
        }
        if(counter > 0) waiter = true;  
        m_shotTimer = m_shotInterval;
        shot_pos = transform.position;
        shot_rot = transform.rotation;
        texts = new List<MyText>();
    }

    public void ThrowArm(float speed)//アーム発射
    {
        var pos = transform.position; // プレイヤーの位置
        var rot = transform.rotation; // プレイヤーの向き
        var arm = Instantiate(m_armPrefab, pos, rot);
        arm.Init(speed, transform.forward);　//アーム初期値設定
    }

    private void ShootText(int i, float speed, char c)//テキスト発射
    {
        var pos = shot_pos; // プレイヤーの位置
        var rot = shot_rot; // プレイヤーの向き

        var pivot = Instantiate(m_pivotPrefab, pos, rot);
        var text = Instantiate(m_textPrefab, pos, rot);
        text.transform.parent = pivot.transform;
        text.Init(i, speed, c.ToString(), textSize);
        texts.Add(text);        
    }

    public int GetMode()//モード取得
    {
        return m_mode;
    }

    private void OpeRecognize(string s)
    {
        int opeCode = -1;
        for (int i = 0; i < keywords.Length; i++)
        {
            if (s == keywords[i])
            {
                opeCode = i;
                break;
            }
        }
        TextManager.Operation(opeCode);
    }
}