using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    private List<Behaviour> behaviours = new List<Behaviour>();

    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = false;
            }
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (isLocalPlayer)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.enabled = focusStatus;
            }
        }
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraRotation>().player = GetComponent<Controller>();
    }
}