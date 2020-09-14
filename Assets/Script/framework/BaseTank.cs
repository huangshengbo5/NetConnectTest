using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTank : MonoBehaviour
{

    private GameObject skin;
    
    //转向速度
    public float steer = 20;

    public float speed = 3f;
    protected Rigidbody rigidbody;

    //炮塔旋转速度
    public float turretSpeed = 30f;

    //炮塔
    public Transform turret;

    //炮管
    public Transform gun;
    //发射点
    public Transform firePoint;

    //炮弹cd时间
    public float firCd = 0.5f;

    //上次开火时间
    public float lastFireTime = 0;

    public float hp = 100f;
    public string id = "";
    public int camp = 0;
   

    public virtual void Init(string skinPath)
    {
        GameObject skinRes = ResManager.LoadPrefab(skinPath);
        skin = (GameObject) Instantiate(skinRes);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;
        rigidbody = gameObject.AddComponent<Rigidbody>();
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0,2.5f,1.47f);
        boxCollider.size = new Vector3(7, 5, 12);

        turret = skin.transform.Find("Turret");
        gun = turret.transform.Find("Gun");
        firePoint = gun.transform.Find("FirePoint");
    }

    public void Update()
    {

    }

    public TankBullet Fire()
    {
        if (IsDie())
        {
            return null;
        }

        GameObject bulletObj = ObjectPool.GetInstance().CreateObj("Bullet");
        bulletObj.transform.localPosition = Vector3.zero;
        bulletObj.transform.localEulerAngles = Vector3.zero;
        rigidbody = bulletObj.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = bulletObj.AddComponent<Rigidbody>();
        }
        
        rigidbody.useGravity = false;
        //rigidbody.isKinematic = false;
        TankBullet bullet = bulletObj.GetComponent<TankBullet>();
        if (bullet == null)
        {
            bullet = bulletObj.AddComponent<TankBullet>();
        }
        bullet.tank = this;
        //位置
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        //更新开火时间
        lastFireTime = Time.time;
        return bullet;
    }


    public bool IsDie()
    {
        return hp <= 0;
    }

    public void Attacked(float att)
    {
        if (IsDie())
        {
            return;
        }

        hp -= att;
        if (IsDie())
        {
            GameObject obj = ResManager.LoadPrefab("explosion");
            GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            explosion.transform.SetParent(transform);
        }
    }
}
