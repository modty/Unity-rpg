using UnityEngine;
// 73
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

    private int damage;

    /// <summary>
    /// 技能目标
    /// </summary>
    public Transform MyTarget { get; set; }

    private Transform source;
    
    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    public void Initialize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
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
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}