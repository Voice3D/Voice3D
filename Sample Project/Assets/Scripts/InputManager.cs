using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    InputField inputField;
    private int opeCode;

    /// <summary>
    /// Startメソッド
    /// InputFieldコンポーネントの取得および初期化メソッドの実行
    /// </summary>
    void Start()
    {

        inputField = GetComponent<InputField>();

        InitInputField();
    }



    /// <summary>
    /// Log出力用メソッド
    /// 入力値を取得してLogに出力し、初期化
    /// </summary>


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
            TextManager.Operation(opeCode);
        }
        else Player.p.ThrowText(inputValue);

        InitInputField();
    }



    /// <summary>
    /// InputFieldの初期化用メソッド
    /// 入力値をリセットして、フィールドにフォーカスする
    /// </summary>


    void InitInputField()
    {

        // 値をリセット
        inputField.text = "";

        // フォーカス
        inputField.ActivateInputField();
    }
}