using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartButton : NetworkBehaviour
{
    public InputField hostIp;
    public GameObject error;
    public Button Lbutton;
    public Button Rbutton;

    public int span;
    private int count = 0;
    private bool flag = false;
    private bool select = false;
    private ColorBlock colors = ColorBlock.defaultColorBlock;

    // Use this for initialization
    void Start()
    {
        colors.normalColor = Color.blue;
        Lbutton.colors = colors;
        Rbutton.colors = ColorBlock.defaultColorBlock;
    }

    // Update is called once per frame
    void Update()
    {
        float lr = Input.GetAxis("LR");
        if (lr == -1)
        {
            Lbutton.colors = colors;
            Rbutton.colors = ColorBlock.defaultColorBlock;
            select = false;
        }
        else if (lr == 1)
        {
            Rbutton.colors = colors;
            Lbutton.colors = ColorBlock.defaultColorBlock;
            select = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (select) PlayAsClient();
            else PlayAsServer();
        }

        if (flag)
        {
            count++;
            if (count > span)
            {
                flag = false;
                error.SetActive(false);
            }
        }
    }

    void PlayAsServer()
    {
        NetworkManager.singleton.StartHost();
    }

    void PlayAsClient()
    {
        NetworkManager.singleton.networkAddress = hostIp.text;
        NetworkManager.singleton.StartClient();
        if (!NetworkManager.singleton.IsClientConnected())
        {
            count = 0;
            flag = true;
            error.SetActive(true);
        }
    }
}
