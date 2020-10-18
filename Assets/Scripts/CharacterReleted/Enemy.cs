using UnityEngine;
// 202

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();
public class Enemy : Character, IInteractable
{
    
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;
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
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }
    
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
    public Transform Select()
    {
        //显示血条
        healthGroup.alpha = 1;

        return hitBox;
    }

    /// <summary>
    /// 当敌人被取消选中
    /// </summary>
    public void DeSelect()
    {
        //隐藏
        healthGroup.alpha = 0;

        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);

        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
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
    public void Interact()
    {
        if (!IsAlive)
        {
            lootTable.ShowLoot();
        }
    }
    
    public void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }
    
    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }

    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }
}