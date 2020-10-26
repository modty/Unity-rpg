using Assets.Scripts.Debuffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour {

    /// <summary>
    /// 技能刚体
    /// </summary>
    private Rigidbody2D myRigidBody;
    
    /// <summary>
    /// 技能移动速度
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// 技能目标
    /// </summary>
    public Transform Target { get; private set; }

    private Character source;

    private float damage;

    private Debuff debuff;

    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
	}

    public void Initialize(Transform target, float damage, Character source, Debuff debuff)
    {
        this.Target = target;
        this.damage = damage;
        this.source = source;
        this.debuff = debuff;
    }

    public void Initialize(Transform target, float damage, Character source)
    {
        this.Target = target;
        this.damage = damage;
        this.source = source;
    }

    private void FixedUpdate()
    {
        if (Target != null)
        {
            // 计算技能方向
            Vector2 direction = Target.position - transform.position;

            // 技能移动
            myRigidBody.velocity = direction.normalized * speed;

            // 计算旋转角度并旋转
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == Target)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source);

            if (debuff != null)
            {
                Debuff clone = debuff.Clone();
                clone.Apply(c);
            }

            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            Target = null;
        }
    }
}
