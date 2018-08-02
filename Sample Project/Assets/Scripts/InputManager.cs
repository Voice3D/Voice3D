using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    InputField inputField;
    private int opeCode;

    //InputFieldコンポーネントの取得および初期化メソッドの実行
    void Start()
    {
        inputField = GetComponent<InputField>();

        InitInputField();
    }

    //入力文字列を処理
    public void InputLogger()
    {
        string inputValue = inputField.text;
        if (inputValue.Length > 0 && inputValue[0] == '/')
        {
            switch (inputValue)
            {

                case "/stop":
                    opeCode = 0;
                    break;
                case "/start":
                    opeCode = 1;
                    break;
            }
            TextManager.tm.Operation(opeCode);
        }
        else Player.p.ThrowText(inputValue);

        InitInputField();
    }

    // InputFieldの初期化用メソッド
    void InitInputField()
    {
        // 値をリセット
        inputField.text = "";

        // フォーカス
        inputField.ActivateInputField();
    }
}