using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// 文字発射デバイスを制御するコンポーネント
public class Player : NetworkBehaviour
{
    public static Player p;
    public MyText m_textPrefab; // 弾のプレハブ
    public VoiceRecognition m_vr;
    public MainPanel main_p;
    public MenuPanel menu_p;
    public Pivot m_pivotPrefab;
    public float m_shotSpeed; // 弾の移動の速さ
    public float m_shotTimer;
    public float m_shotInterval;
    public int MaxLength;
    public int maxStrs;
    public List<List<MyText>> inventory;
    public bool menu = false;

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
    private int textSize = 1;
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

    private void Start()
    {
        inventory = new List<List<MyText>>();
        Hud.instance.m_player = this;
        m_vr = VoiceRecognition.instance;
    }

    // 毎フレーム呼び出される関数
    private void Update()
    {
        //照準の向き、及びアーム発射制御
        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30))
        {
            transform.LookAt(hit.point);
        }
        */

        //音声認識の開始、停止、モードの制御
        if (!triggerOn && Input.GetAxis("Ltrigger") == 1)
        {
            triggerOn = true;
            RpcRecog(true);
        }
        if (triggerOn && Input.GetAxis("Ltrigger") == 0)
        {
            triggerOn = false;
            RpcRecog(false);
        }
        if (!triggerOn && Input.GetButtonDown("change")) m_mode = (m_mode + 1) % keywords.Length;

        //テキスト発射制御
        if (waiter)
        {
            //テキスト発射間隔の調整
            m_shotTimer += Time.deltaTime;
            if (m_shotTimer < m_shotInterval*textSize) return;
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
        if (waiter || TextManager.textStop) return;//テキスト発射中もしくは文字の回転が停止中の場合は文字を発射しない
        if (m_mode == 1)//音声コマンド
        {
            OpeRecognize(s);
            return;
        }
        waitS = s;
        counter = waitS.Length;
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

    private void OpeRecognize(string s)//入力の音声コマンドから命令の種類を決定し実行
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

    public void AddInventory(List<MyText> list)
    {
        string str = "";
        int i;
        inventory.Add(list);
        for (i = 0; i < list.Count; i++)
        {
            str += list[i].GetComponent<TextMesh>().text;
        }
        main_p.SetInventory(str);
    }

    public void RemoveInventory(int id)
    {
        inventory.RemoveAt(id);
        main_p.PickInventory(id);        
    }

    //文字を取り出す
    public void PickText(int id)
    {
        List<MyText> temp = inventory[id];
        Vector3 basePos = transform.parent.position + 2*transform.parent.forward;
        basePos.y += 1.5f;
        //文字を見えるようにする
        //文字の位置修正
        for (int i=0;i<temp.Count;i++)
        {
            temp[i].gameObject.SetActive(true);
            temp[i].transform.position = new Vector3(basePos.x+i, basePos.y, basePos.z);
            temp[i].transform.rotation = transform.parent.rotation;
            temp[i].rotation = false;
            temp[i].GetComponent<BoxCollider>().isTrigger = false;
        }

        //インベントリから削除
        inventory.RemoveAt(id);
        main_p.PickInventory(id);
    }

    [ClientRpc]
    private void RpcRecog(bool flag)
    {
        if (flag) m_vr.StartRecognition();
        else m_vr.StopRecognition();
    }
}