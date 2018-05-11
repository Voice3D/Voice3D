﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour {
    private int next=-1;
    private int pre_id=-1;
    public static TextManager tm;
    public int m_top, m_bottom, m_dev;//文字列が周回する領域の、一番高いところ、低いところ、何列に分けるか
    public static Dictionary<int, List<MyText>> strs = new Dictionary<int, List<MyText>>();//空間に存在する文字列を格納
    public static List<List<MyText>> LineManager = new List<List<MyText>>();//外側周回中の文字列を格納（indexが小さい方が上）

    private void Awake()
    {
        tm = this;
    }

    // Use this for initialization
    void Start()
    {

    }

	// Update is called once per frame
	void Update () {
		
	}

    //テキスト位置更新
    public void PosiText(int t_id, int t_size)
    {
        if (pre_id != t_id)
        {
            pre_id = t_id;
            //foreach (int key in strs.Keys) Debug.Log(key+":"+t_id);
            for (int i = 0; i < LineManager.Count; i++)
            {
                for (int j = 0; j < LineManager[i].Count; j++)
                {
                    LineManager[i][j].GetComponent<MyText>().transform.localPosition -= new Vector3(0, (m_top - m_bottom) / m_dev*t_size, 0);
                }
            }
            //Debug.Log(strs[t_id]);
            if(LineManager.Count==m_dev) LineManager.RemoveAt(m_dev);
            if (LineManager.Count != 0) LineManager.Insert(0, strs[t_id]);
            else LineManager.Add(strs[t_id]);
            
        }

        return;

    }

    //音声コマンド処理
    public static void Operation(int code) {
        switch (code) {
            case 0:
                for (int i = 0; i < LineManager.Count; i++) {
                    for (int j = 0; j < LineManager[i].Count; j++) {
                        LineManager[i][j].m_stop = true;
                    }

                }
                break;
            case 1:
                for (int i = 0; i < LineManager.Count; i++)
                {
                    for (int j = 0; j < LineManager[i].Count; j++)
                    {
                        LineManager[i][j].m_stop = false;
                    }

                }
                break;
            default:
                Debug.Log("erorr: opeCode = "+code);
                break;
        }
        return;
    }




}
