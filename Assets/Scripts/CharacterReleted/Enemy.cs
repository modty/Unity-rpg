using UnityEngine;
// 155
public class Enemy : NPC
{
    /// <summary>
    /// 血条画布
    /// </summary>
    [SerializeField]
    private CanvasGroup healthGroup;

    /// <summary>
    /// 敌人当前状态
    /// </summary>
    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    
    /// <summary>
    /// 敌人攻击范围
    /// </summary>
    public float MyAttackRange { get; set; }

    /// <summary>
    /// 距离上次攻击过去的时间
    /// </summary>
    public float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private float initAggroRange;

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }
    protected void Awake()
    {
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 1;
        ChangeState(new IdleState());
    }
    
    protected override void Update()
    {
        if (IsAlive)
        {

            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();         
        }

        base.Update();

    }
    /// <summary>
    /// 当敌人被选中
    /// </summary>
    public override Transform Select()
    {
        //显示血条
        healthGroup.alpha = 1;

        return base.Select();
    }

    /// <summary>
    /// 当敌人被取消选中
    /// </summary>
    public override void DeSelect()
    {
        //隐藏
        healthGroup.alpha = 0;

        base.DeSelect();
    }
    public override void TakeDamage(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            SetTarget(source);

            base.TakeDamage(damage, source);

            OnHealthChanged(health.MyCurrentValue);
        }
    }
    /// <summary>
    /// 改变敌人的状态
    /// </summary>
    /// <param name="newState">新状态</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null) // 确保状态不为空
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }
    public void SetTarget(Transform target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }
    public override void Interact()
    {
        if (!IsAlive)
        {
            lootTable.ShowLoot();
        }
    }
}