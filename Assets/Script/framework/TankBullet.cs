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
            //hitTank.Attacked(35f);
            SendMsgHit(tank, hitTank);
        }
        //显示爆炸效果
        //GameObject explode = ResManager.LoadPrefab("fire");
        //Instantiate(explode, transform.position, transform.rotation);
        //gameObject.SetActive(false);
    }

    public void SendMsgHit(BaseTank tank, BaseTank hitTank)
    {
        if (hitTank==null || tank == null)
        {
            return;
        }

        if (tank.id != GameMain.id)
        {
            return;
        }
        MsgHit msg = new MsgHit();
        msg.targetId = hitTank.id;
        msg.id = tank.id;
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;
        FrameWorkNetManager.Send(msg);
    }
}
