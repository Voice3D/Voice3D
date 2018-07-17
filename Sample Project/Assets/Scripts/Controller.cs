using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
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
    int sign;
    Vector3 angle;

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


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //UnityちゃんのAnimatorにアクセスする
        animator = GetComponent<Animator>();
        prePosi = transform.position;
    }

    void Update()
    {
        rb.angularVelocity = new Vector3(0,0,0);

        //表示、非表示の変更
        if (Input.GetButtonDown("Menu")) menu = !menu;
        transform.GetChild(1).gameObject.SetActive(menu);
        transform.GetChild(0).gameObject.SetActive(!menu);

        //地面に触れている場合発動
        if (ground)
        {
            //ユニティちゃんの移動
            //float dx = Input.GetAxisRaw("Horizontal");
            //float dy = Input.GetAxisRaw("Vertical");

            //viveコントローラー用
            Vector2 posi = TouchpadExmpleleft.position;
            float dx = posi.x;
            float dy = posi.y;



            if (Mathf.Abs(dx)<0.3 && Mathf.Abs(dy)<0.3)
            {
                //何もキーを押していない時はアニメーションをオフにする
                rb.velocity = new Vector3(0, 0, 0);
                animator.SetBool("Running", false);
            }
            else
            {
                if (dy < 0) sign = -1;
                else sign = 1;
                angle = Camera.main.transform.eulerAngles;
                float dir = angle.y - Vector3.Angle(Vector3.right, new Vector3(dx, 0, dy))*sign+90;
                transform.localEulerAngles = new Vector3(0, dir, 0);
                rb.velocity = speed * transform.forward;
                transform.localEulerAngles = new Vector3(0, angle.y, 0);
                animator.SetBool("Running", true);
            }
            Camera.main.transform.parent.transform.position += transform.position - prePosi;
            //スペースキーでジャンプする
            /*
            if (Input.GetButtonDown("Jump"))
            {               
                animator.SetBool("Jumping", true);
                //上方向に向けて力を加える
                rb.AddForce(new Vector3(0, thrust, 0));
                ground = false;               
            }
            else
            {
                animator.SetBool("Jumping", false);
            }
            */
        }
        prePosi = transform.position;
        
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
}