using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

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
    /// 所有技能预制件
    /// </summary>
    [SerializeField]
    private GameObject[] spellPrefab;

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
    
    /// <summary>
    /// 角色目标
    /// </summary>
    public Transform MyTarget { get; set; }

    private SpellBook spellBook;
    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
        mana.Initialize(initMana, initMana);
        
        base.Start();
    }

    protected override void Update ()
    {

        GetInput();

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
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }
    }
    
    /// <summary>
    /// 攻击协程
    /// </summary>
    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);
        
        isAttacking = true; // 确认攻击状态

        myAnimator.SetBool("attack", isAttacking); // 调用攻击动画

        yield return new WaitForSeconds(newSpell.MyCastTime); // 方便Debug
        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget,newSpell.MyDamage);

        }
        StopAttack(); // 停止攻击
    }
    /// <summary>
    /// 发射技能
    /// </summary>
    public void CastSpell(int spellIndex)
    {
        Block();

        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight()) //Chcks if we are able to attack
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        }
    }

    /// <summary>
    /// 检测目标是否在视线上
    /// </summary>
    private bool InLineOfSight()
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
}
