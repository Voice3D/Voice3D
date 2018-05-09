using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour {
    private int next=-1;
    private int pre_id=-1;
    public static TextManager tm;
    public int m_top, m_bottom, m_dev;
    public static Dictionary<int, List<Text>> strs = new Dictionary<int, List<Text>>();
    public static List<List<Text>> LineManager = new List<List<Text>>();

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

    public void PosiText(int t_id)
    {
        if (pre_id != t_id)
        {
            pre_id = t_id;
            //foreach (int key in strs.Keys) Debug.Log(key+":"+t_id);
            for (int i = 0; i < LineManager.Count; i++)
            {
                for (int j = 0; j < LineManager[i].Count; j++)
                {
                    LineManager[i][j].GetComponent<Text>().transform.localPosition -= new Vector3(0, (m_top - m_bottom) / m_dev, 0);
                }
            }
            //Debug.Log(strs[t_id]);
            if(LineManager.Count==m_dev) LineManager.RemoveAt(m_dev);
            if (LineManager.Count != 0) LineManager.Insert(0, strs[t_id]);
            else LineManager.Add(strs[t_id]);
            
        }

        return;

    }

    public static void Operation(string str) {
        switch (str) {

            case "/stop":
                for (int i = 0; i < LineManager.Count; i++) {
                    for (int j = 0; j < LineManager[i].Count; j++) {
                        LineManager[i][j].m_stop = true;
                    }

                }
                break;
            case "/start":
                for (int i = 0; i < LineManager.Count; i++)
                {
                    for (int j = 0; j < LineManager[i].Count; j++)
                    {
                        LineManager[i][j].m_stop = false;
                    }

                }
                break;
        }


        return;
    }




}
