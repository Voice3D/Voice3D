using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    public int keyCount;
    private DictationRecognizer dicRecognizer;
    private KeywordRecognizer keyRecognizer;
    private string[] keywords = {"すとっぷ", "すたーと"};

    void Start()
    {
        keyCount = keywords.Length;
        dicRecognizer = new DictationRecognizer();
        dicRecognizer.InitialSilenceTimeoutSeconds = 10;
        // 確定
        dicRecognizer.DictationResult += (text, confidence) =>
        {
            gameObject.GetComponent<UnityEngine.UI.Text>().text = text;
            GUIUtility.systemCopyBuffer = text;
            //			System.Diagnostics.Process.Start(path);
            Player.p.ThrowText(text);
        };
        // 推測
        dicRecognizer.DictationHypothesis += (text) => {
            // 推測時にする処理
         
        };
        // 停止時
        dicRecognizer.DictationComplete += (completeCause) =>
        {
            // 要因がタイムアウトなら再び起動
            if (completeCause == DictationCompletionCause.TimeoutExceeded)
                dicRecognizer.Start();
        };
        keyRecognizer = new KeywordRecognizer(keywords);
        keyRecognizer.OnPhraseRecognized += OnPhraseRecognized;
    }

    void update()
    {
    }

    public void StartRecognition(int i)
    {
        Debug.Log("start:"+i);
        switch (i)
        {
            case 0:
                dicRecognizer.Start();
                break;

            case 1:
                keyRecognizer.Start();
                break;
        }
    }

    public void StopRecognition(int i)
    {
        Debug.Log("stop:" + i);
        switch (i)
        {
            case 0:
                dicRecognizer.Stop();
                break;

            case 1:
                keyRecognizer.Stop();
                break;
        }
    }



    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        int opeCode = -1;
        for (int i = 0; i < keyCount; i++)
        {
            if (args.text == keywords[i])
            {
                opeCode = i;
                break;
            }
        }
        TextManager.Operation(opeCode);        
    }
}
