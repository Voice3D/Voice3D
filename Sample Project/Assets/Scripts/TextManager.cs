using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour {
    private int next=0;
    private int pre_id=-1;
    //private List<MyText> str;
    //private Dictionary<int, int> LineId = new Dictionary<int, int>();
    private int[] line;
    private List<Pivot>[] LineManager;
    public static bool textStop = false;
    public static TextManager tm;
    public int m_top, m_bottom, m_dev;//文字列が周回する領域の、一番高いところ、低いところ、何列に分けるか
    public float heightPL;//一列当たりの高さ
    public static Dictionary<int, List<MyText>> strs = new Dictionary<int, List<MyText>>();//空間に存在する文字列を格納
    //public static List<List<MyText>> LineManager = new List<List<MyText>>();//外側周回中の文字列を格納（indexが小さい方が上）

    // Use this for initialization
    void Start()
    {
        tm = this;
        heightPL = (float)(m_top - m_bottom) / m_dev;
        line = new int[m_dev];
        LineManager = new List<Pivot>[m_dev];
        for (int i = 0; i < m_dev; i++)
            line[i] = -1;
    }

    //テキスト位置更新
    public int PosiText(int t_id, int t_size, Pivot pivot)
    {
        int l = -1;
        for (int i = 0; i < m_dev; i++)
            if (line[i] == t_id) l = i;

        if (l == -1)
        {
            List<Pivot> temp;
            for (int i = 0; i < t_size; i++)
            {
                if ((temp = LineManager[m_dev - 1 - i]) != null)
                    foreach (Pivot t in temp)
                        Destroy(t.gameObject);
            }

            for (int i = 0; i < m_dev; i++)
            {
                var ln = m_dev - t_size - i - 1;
                if (ln < 0)
                {
                    line[ln + t_size] = -2;
                    LineManager[ln + t_size] = null;
                    continue;
                }
                if (LineManager[ln] != null)
                    for (int j = 0; j < LineManager[ln].Count; j++)
                        LineManager[ln][j].transform.position -= new Vector3(0, heightPL * t_size, 0);
                line[ln + t_size] = line[ln];
                LineManager[ln + t_size] = LineManager[ln];
            }
            line[0] = t_id;
            LineManager[0] = new List<Pivot>();           
            LineManager[0].Add(pivot);
            l = 0;
        }
        else
        {
            LineManager[l].Add(pivot);
        }
        return l;
    }

    //音声コマンド処理
    public void Operation(int code) {
        Debug.Log("ope: "+code);
        switch (code) {
            case 0:
                for (int i = 0; i < m_dev; i++) {
                    if (LineManager[i] == null) continue;
                    for (int j = 0; j < LineManager[i].Count; j++)
                    {
                        if (LineManager[i][j] != null)
                        {
                            LineManager[i][j].m_stop = true;
                            Debug.Log("stop");
                        }
                    }
                }
                textStop = true;
                break;
            case 1:
                for (int i = 0; i < m_dev; i++)
                {
                    if (LineManager[i] == null) continue;
                    for (int j = 0; j < LineManager[i].Count; j++)
                    {
                        if (LineManager[i][j] != null)
                        {
                            LineManager[i][j].m_stop = false;
                            Debug.Log("move");
                        }
                    }
                }
                textStop = false;
                break;
            default:
                Debug.Log("erorr: opeCode = "+code);
                break;
        }
        return;
    }
}