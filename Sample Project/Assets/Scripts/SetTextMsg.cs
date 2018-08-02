using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetTextMsg : MessageBase {
    public int id;
    public char c;
    public int size;

    public SetTextMsg(int i, char c, int s)
    {
        this.id = i;
        this.c = c;
        this.size = s;
    }
}
