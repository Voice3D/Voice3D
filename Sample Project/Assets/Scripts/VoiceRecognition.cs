using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    public float maxFreq;
    public float maxVol;

    public float sensitivity = 100;     //感度.音量の最大値.
    public float vol;             //音量.
    float lastVol;          //前フレームの音量.
    AudioSource aud;

    [Range(0, 0.95f)]           //最大1にできてしまうと全く変動しなくなる.
    public float lastVolInfluence;  //前フレームの影響度合い.

    private DictationRecognizer dicRecognizer;
    public static VoiceRecognition instance;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        // マイク名、ループするかどうか、AudioClipの秒数、サンプリングレート を指定する
        if (Microphone.devices == null || Microphone.devices.Length == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        aud.clip = Microphone.Start(null, true, 3, 44100);
        aud.loop = true;
        aud.mute = false;
        while (!(Microphone.GetPosition(null) > 0)) { }
        aud.Play();

        instance = this;
        dicRecognizer = new DictationRecognizer();
        dicRecognizer.InitialSilenceTimeoutSeconds = 10;
        // 確定
        dicRecognizer.DictationResult += (text, confidence) =>
        {
            gameObject.GetComponent<UnityEngine.UI.Text>().text = text;
            GUIUtility.systemCopyBuffer = text;
            //			System.Diagnostics.Process.Start(path);
            if (maxFreq < 200) MyText.str = 1;
            else if (maxFreq > 2000) MyText.str = 0.5f;
            else
            {
                MyText.str = -(maxFreq - 200) / 20 + 00 + 1f;
            }
            var size = (int) (maxVol/7);
            if (size > 4) size = 4;
            else if (size == 0) size = 1;

            Debug.Log("vol: " + maxVol + ", " + maxFreq);

            Player.p.ThrowText(text, size);
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
    }

    void Update()
    {
        //Debug.Log("update");
        //Debug.Log(dicRecognizer.Status);
        if (dicRecognizer.Status == SpeechSystemStatus.Running)
        {
            GetAveragedVolume();
            //Debug.Log("vol: "+vol);            
        }
    }

    public void StartRecognition()
    {
        if(dicRecognizer.Status == SpeechSystemStatus.Stopped) dicRecognizer.Start();
        maxFreq = 0;
        maxVol = 0;
    }

    public void StopRecognition()
    {
        if (dicRecognizer.Status == SpeechSystemStatus.Running) dicRecognizer.Stop();
    }

    void GetAveragedVolume()
    {
        Debug.Log("rate: " + AudioSettings.outputSampleRate);
        float[] data = new float[256];
        float[] spectrum = new float[256];
        float a = 0;
        float max = 0.0f;
        int index = 0;
        AudioListener.GetOutputData(data, 0);

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        for (int i=0;i<256;i++)
        {
            if (i != 0 && max < Mathf.Abs(spectrum[i]))
            {
                //Debug.Log(": "+i);
                max = Mathf.Abs(spectrum[i]);
                index = i;
            }
            a += Mathf.Abs(data[i]);
        }

        if (maxVol < a)
        {
            maxVol = a;
            maxFreq = index * AudioSettings.outputSampleRate / 512;
        }
        var pitch = index * AudioSettings.outputSampleRate / 512;
        if (pitch < 200) MyText.str = 1;
        else if (pitch > 2000) MyText.str = 0.5f;
        else
        {
            MyText.str = -(pitch - 200) / 20+00 + 1f;
        }
        //Debug.Log("str: " +index+", "+ pitch+", "+a);
    }
}
