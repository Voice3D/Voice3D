using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private List<MyText> myTexts = new List<MyText>();
    private int armStop = 1;
    private int have = 0;
    public int m_cylR;
    public float speed;
    public bool textStop = false;

    // Update is called once per frame
    void Update()
    {
        //選択した文字の回収
        if (have == 1 && Input.GetAxis("Rtrigger") == 0)
        {
            have = 2;
            foreach (MyText i in myTexts)
            {
                i.transform.parent.parent = transform;
            }
        }
        // 移動する
        if (armStop == 2 && (Vector3.Distance(transform.position, new Vector3(0, transform.position.y, 0)) < m_cylR)) armStop = 0;        
        if (armStop != 2 && Input.GetAxis("Rtrigger") == 1)
        {
            armStop = 0;
            transform.position += transform.parent.forward * speed;
        }
        if (armStop != 1 && Input.GetAxis("Rtrigger") == 0)
        {
            armStop = 0;
            transform.position -= transform.parent.forward * speed;
        }
        if (armStop == 2 && Vector3.Distance(transform.position, new Vector3(0, transform.position.y, 0)) > m_cylR+0.5)
        {
            transform.position -= transform.parent.forward * speed;
        }

        if (armStop==0)
        {
            if (Vector3.Distance(transform.position, new Vector3(0, transform.position.y, 0)) > m_cylR)
            {
                armStop = 2;
            }
            else if (Vector3.Distance(transform.localPosition, new Vector3(0, transform.localPosition.y, 0)) < 0.5)
            {
                have = 0;
                armStop = 1;
                var count = myTexts.Count;
                Debug.Log(count);
                foreach(MyText i in myTexts)
                {
                    i.gameObject.SetActive(false);
                    i.transform.parent.parent = null;
                }
                myTexts.Clear();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //文字を選択状態にする
        if (other.gameObject.tag == "Text" && Input.GetButtonDown("Fire1") && have != 2 && TextManager.textStop)
        {
            other.gameObject.GetComponent<MyText>().rotation = true;
            myTexts.Add(other.gameObject.GetComponent<MyText>());
            have = 1;
        }
    }
}

