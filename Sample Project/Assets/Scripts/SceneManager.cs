using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public string cylTag = "TextManager";

    public GameObject unitychan = GameObject.Find("unitychan");

    // Update is called once per frame  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //マウスクリックした場所からRayを飛ばし、オブジェクトがあればtrue 
            if (Physics.Raycast(unitychan.transform.position, ray.direction, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag(cylTag))
                {
                    //hit.collider.gameObject.GetComponent<TextManager>().;
                }
            }
        }
    }

}