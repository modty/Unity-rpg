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
    public Transform MyTarget { get; set; }

    
    private int damage;
    
    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    public void Initialize(Transform target, int damage)
    {
        this.MyTarget = target;
        this.damage = damage;
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            // 计算技能方向
            Vector2 direction = MyTarget.position - transform.position;

            // 技能移动
            myRigidBody.velocity = direction.normalized * speed;

            // 计算旋转角度并旋转
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            speed = 0;
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}