using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }
    /// <summary>
    /// 魔法值
    /// </summary>
    [SerializeField]
    private Stat mana;
    
    /// <summary>
    /// 初始魔法值
    /// </summary>
    private float initMana = 50;

    /// <summary>
    /// 用于阻挡玩家视线的方块数组
    /// </summary>
    [SerializeField]
    private Block[] blocks;

    
    /// <summary>
    /// 技能生效的所有地点
    /// </summary>
    [SerializeField]
    private Transform[] exitPoints;

    /// <summary>
    /// 跟踪使用2的默认退出点
    /// </summary>
    private int exitIndex = 2;
    

    private Vector3 min, max;

    protected override void Start()
    {
        mana.Initialize(initMana, initMana);
        
        base.Start();
    }

    protected override void Update ()
    {
        GetInput();
        // 初始化角色位置
        var position = transform.position;
        position = new Vector3(Mathf.Clamp(position.x, min.x, max.x), 
            Mathf.Clamp(position.y, min.y, max.y), 
            position.z);
        transform.position = position;

        base.Update();
    }


    // 监听键盘输入
    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"]))
        {
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"]))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"]))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }
        if (IsMoving)
        {
            StopAttack();
        }
        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);

            }
        }
    }
    
    /// <summary>
    /// 设置角色移动的地图范围
    /// </summary>
    /// <param name="min">最小移动位置</param>
    /// <param name="max">最大移动位置</param>
    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// 攻击协程
    /// </summary>
    private IEnumerator Attack(string spellName)
    {
        Transform currentTarget = MyTarget;

        Spell newSpell = SpellBook.MyInstance.CastSpell(spellName);
        
        IsAttacking = true; // 确认攻击状态

        MyAnimator.SetBool("attack", IsAttacking); // 调用攻击动画

        yield return new WaitForSeconds(newSpell.MyCastTime); // 方便Debug
        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget,newSpell.MyDamage,transform);
        }
        StopAttack(); // 停止攻击
    }
    /// <summary>
    /// 发射技能
    /// </summary>
    public void CastSpell(string spellName)
    {
        Block();

        if (MyTarget != null && !IsAttacking && !IsMoving && InLineOfSight()) //Chcks if we are able to attack
        {
            attackRoutine = StartCoroutine(Attack(spellName));
        }
    }

    /// <summary>
    /// 检测目标是否在视线上
    /// </summary>
    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            // 目标方向
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            // 向目标方向发射射线
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position),256);

            // 如果射线没有碰撞到视线方块，就发射技能
            if (hit.collider == null)
            {
                return true;
            }
        }

        // 碰撞到方块就不发射
        return false;
    }

    /// <summary>
    /// 根据角色位置更新视线方块位置
    /// </summary>
    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }
    
    /// <summary>
    /// 重载停止攻击方法
    /// </summary>
    public void StopAttack()
    {

        // 停止技能释放
        SpellBook.MyInstance.StopCating();

        IsAttacking = false; // 修改攻击状态

        MyAnimator.SetBool("attack", IsAttacking); // 停止攻击动画

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }
}
