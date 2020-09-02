using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public float speed = 50f;
    //发射者
    public BaseTank tank;

    //炮弹模型
    public GameObject skin;

    private Rigidbody rigitBody;
    public float lifeTime = 10f;



	// Update is called once per frame
	void Update () 
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        GameObject collObj = collisionInfo.gameObject;
        BaseTank hitTank = collObj.GetComponent<BaseTank>();

        if (collObj.tag == null )
        {
            return;
        }

        if (hitTank ==  tank)
        {
            return; 
        }

        if (hitTank != null)
        {
            hitTank.Attacked(35f);
        }
        //显示爆炸效果
        GameObject explode = ResManager.LoadPrefab("fire");
        Instantiate(explode, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
