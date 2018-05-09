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
            /*
            //上下左右のキーでの移動、向き、アニメーション
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //移動(X軸、Y軸、Z軸）
                rb.velocity = new Vector3(speed, 0, 0);
                //向き(X軸、Y軸、Z軸）
                transform.rotation = Quaternion.Euler(0, 90, 0);
                //アニメーション
                animator.SetBool("Running", true);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector3(-speed, 0, 0);
                transform.rotation = Quaternion.Euler(0, 270, 0);
                animator.SetBool("Running", true);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.velocity = new Vector3(0, 0, speed);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("Running", true);
            }

            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.velocity = new Vector3(0, 0, -speed);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("Running", true);
            }*/
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
                // transform.LookAt(transform.localPosition+new Vector3(dx, 0, dy));
                rb.velocity = speed * transform.forward;
                //transform.rotation = Quaternion.Euler(0, angle.y+Mathf.Rad2Deg*Mathf.Atan(dy/dx)-90, 0);
                animator.SetBool("Running", true);
            }
            Camera.main.GetComponent<Rigidbody>().velocity = rb.velocity;
            //スペースキーでジャンプする
            if (Input.GetButtonDown("Jump"))
            {
                //Debug.Log("速度: " + rb.velocity+":"+ Camera.main.GetComponent<Rigidbody>().velocity);
                //Debug.Log(dx + ":" + dy);
                Debug.Log(Input.GetAxis("Rtrigger"));
                animator.SetBool("Jumping", true);
                //上方向に向けて力を加える
                rb.AddForce(new Vector3(0, thrust, 0));
                ground = false;
                //Debug.Log(Vector3.Angle(Vector3.right, new Vector3(dx, 0, dy)) - 90);
            }
            else
            {
                animator.SetBool("Jumping", false);
            }
        }
    }

     //別のCollider、今回はPlaneに触れているかどうかを判断する
    void OnCollisionStay(Collision col)
    {
        //Debug.Log("check");
        ground = true;
    }
}