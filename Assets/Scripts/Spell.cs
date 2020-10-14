using UnityEngine;

public class Spell : MonoBehaviour {

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

    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

    }
	
    void Update () {
		
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
        Debug.Log(collision.tag+"|||"+MyTarget.tag);
        if (collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}