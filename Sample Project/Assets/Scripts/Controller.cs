using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controller : NetworkBehaviour
{
    Rigidbody rb;
    //移動スピード
    public float speed = 4f;
    //ジャンプ力
    public float thrust = 100;
    //Animatorを入れる変数
    private Animator animator;
    //Planeに触れているか判定するため
    private bool ground=false;
    private Vector3 prePosi;
    private bool menu = false;
    private float deg=6;
    private bool rotFlag = false;
    //private short setTextNo = MsgType.Highest+1;
    [SerializeField]
    private GameObject[] models;

    private Pivot pivot;
    int sign;
    Vector3 angle;

    public Pivot m_prefab;

    public bool ikActive = false;
    public Transform bodyObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;
    public Transform leftHandObj = null;
    public Transform rightHandObj = null;
    public Transform lookAtObj = null;

    public float leftFootWeightPosition = 1;
    public float leftFootWeightRotation = 1;

    public float rightFootWeightPosition = 1;
    public float rightFootWeightRotation = 1;

    public float leftHandWeightPosition = 1;
    public float leftHandWeightRotation = 1;

    public float rightHandWeightPosition = 1;
    public float rightHandWeightRotation = 1;

    public float lookAtWeight = 1.0f;

    public static Controller uc;

    [SerializeField]
    private GameObject player, menuPan, light;

    [SyncVar]
    public bool textStop = false;


    void Start()
    {
        transform.position = new Vector3(0, 1, 0);
        rb = GetComponent<Rigidbody>();
        //UnityちゃんのAnimatorにアクセスする
        animator = GetComponent<Animator>();
        prePosi = transform.position;
        //Camera.main.GetComponent<CameraRotation>().player = this;
    }

    void Update()
    {
        rb.angularVelocity = new Vector3(0,0,0);

        //表示、非表示の変更
        if (Input.GetButtonDown("Menu")) menu = !menu;
        menuPan.SetActive(menu);
        player.SetActive(!menu);
        light.SetActive(!menu);

        //地面に触れている場合発動
        if (ground)
        {
            //ユニティちゃんの移動
            var posi = TouchpadExmpleleft.position;
            float dx = posi.x;
            float dy = posi.y;
            Debug.Log("xy: "+dx+", "+dy);
           
            if (Mathf.Abs(dx)<0.1 && Mathf.Abs(dy)<0.1)
            {
                //何もキーを押していない時はアニメーションをオフにする
                Debug.Log("stop");
                rb.velocity = new Vector3(0, 0, 0);
                animator.SetBool("Running", false);
            }
            else
            {
                if (dx < 0) sign = -1;
                else sign = 1;
                //angle = Camera.main.transform.eulerAngles;
                float dir = (Vector3.Angle(Vector3.forward, new Vector3(dx, 0, dy))*sign)%360;
                transform.eulerAngles += new Vector3(0, dir, 0);
                rb.velocity = speed * transform.forward;
                transform.eulerAngles -= new Vector3(0, dir, 0);
                animator.SetBool("Running", true);
            }

            //Camera.main.transform.position += transform.position - prePosi;
            //Debug.Log("lr: "+ Input.GetAxis("rotL")+", "+ Input.GetAxis("rotR"));              

            if (!rotFlag)
            {
                if (Input.GetAxis("rotL") > 0.9)
                {
                    deg = (deg + 1) % 12;
                    rotFlag = true;
                }
                else if (Input.GetAxis("rotR") > 0.9)
                {
                    deg = (deg - 1) % 12;
                    rotFlag = true;
                }       
                transform.eulerAngles = new Vector3(0, deg * 30f-180f, 0);
                CameraPosi.cp.transform.eulerAngles = new Vector3(0, deg * 30f-180, 0);
            }
            else if ((Input.GetAxis("rotR") < 0.9) && (Input.GetAxis("rotL") < 0.9))
            { rotFlag = false; Debug.Log("rot false"); }
            
        }
        prePosi = transform.position;
        Player.p.transform.position = rightHandObj.position;
        Player.p.transform.rotation = rightHandObj.rotation;
    }

    void OnCollisionStay(Collision col)//ジャンプをもし実装するなら使うかも
    {
        ground = true;
    }

    void OnGUI()
    {
        GUILayout.Label("Activate IK and move the Effectors around in Scene View");
        ikActive = GUILayout.Toggle(ikActive, "Activate IK");
    }


    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (ikActive)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeightRotation);

                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeightRotation);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeightRotation);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeightRotation);

                animator.SetLookAtWeight(lookAtWeight, 0.3f, 0.6f, 1.0f, 0.5f);

                if (bodyObj != null)
                {
                    animator.bodyPosition = bodyObj.position;
                    animator.bodyRotation = bodyObj.rotation;
                }

                if (leftFootObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
                }

                if (rightFootObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);
                }

                if (leftHandObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }

                if (rightHandObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }

                if (lookAtObj != null)
                {
                    animator.SetLookAtPosition(lookAtObj.position);
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);

                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

                animator.SetLookAtWeight(0.0f);

                if (bodyObj != null)
                {
                    bodyObj.position = animator.bodyPosition;
                    bodyObj.rotation = animator.bodyRotation;
                }

                if (leftFootObj != null)
                {
                    leftFootObj.position = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
                    leftFootObj.rotation = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
                }

                if (rightFootObj != null)
                {
                    rightFootObj.position = animator.GetIKPosition(AvatarIKGoal.RightFoot);
                    rightFootObj.rotation = animator.GetIKRotation(AvatarIKGoal.RightFoot);
                }

                if (leftHandObj != null)
                {
                    leftHandObj.position = animator.GetIKPosition(AvatarIKGoal.LeftHand);
                    leftHandObj.rotation = animator.GetIKRotation(AvatarIKGoal.LeftHand);
                }

                if (rightHandObj != null)
                {
                    rightHandObj.position = animator.GetIKPosition(AvatarIKGoal.RightHand);
                    rightHandObj.rotation = animator.GetIKRotation(AvatarIKGoal.RightHand);
                }

                if (lookAtObj != null)
                {
                    lookAtObj.position = animator.bodyPosition + animator.bodyRotation * new Vector3(0, 0.5f, 1);
                }
            }
        }
    }

    public override void OnStartLocalPlayer()
    {
        //Camera.main.GetComponent<CameraRotation>().player = GetComponent<Controller>();
        Player.p = transform.GetChild(0).GetComponent<Player>();
        Controller.uc = this;
        CameraPosi.cp.setObj();
        //transform.position = new Vector3(0, 1, 0);
        
        for (int i = 0; i < models.Length; i++)
            models[i].SetActive(false);
            
    }

    [Command]
    public void CmdGenText(Vector3 pos, Quaternion rot, int i, char c, int textSize)
    {
        pivot = Instantiate(m_prefab, pos, rot);
        NetworkServer.Spawn(pivot.gameObject);
        pivot.RpcSetText(i, c, textSize);
    }

    [Client]
    public void VrOp(bool flag)
    {
        Player.p.Recog(flag);
    }
}