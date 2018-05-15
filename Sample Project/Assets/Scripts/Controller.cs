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
    bool ground;
    int sign;
    Vector3 angle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //UnityちゃんのAnimatorにアクセスする
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        rb.angularVelocity = new Vector3(0,0,0);
        //地面に触れている場合発動
        if (ground)
        {
            //ユニティちゃんの移動
            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
           
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
            Camera.main.GetComponent<Rigidbody>().velocity = rb.velocity;
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
    }

    void OnCollisionStay(Collision col)//ジャンプを模もし実装するなら使うかも
    {
        ground = true;
    }
}