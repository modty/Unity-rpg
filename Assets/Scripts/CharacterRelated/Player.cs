using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
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

    private List<Enemy> attackers = new List<Enemy>();

    /// <summary>
    /// 魔法值
    /// </summary>
    [SerializeField]
    private Stat mana;

    [SerializeField]
    private Stat xpStat;

    [SerializeField]
    private Text levelText;

    /// <summary>
    /// 初始魔法值
    /// </summary>
    private float initMana = 50;

    private Vector2 initPos;

    [SerializeField]
    private SpriteRenderer[] gearRenderers;

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

    [SerializeField]
    private Animator ding;

    [SerializeField]
    private Transform minimapIcon;

    [SerializeField]
    private Camera mainCam;

    /// <summary>
    /// 跟踪使用2的默认退出点
    /// </summary>
    private int exitIndex = 2;

    public Coroutine MyInitRoutine { get; set; }

    private List<IInteractable> interactables = new List<IInteractable>();

    #region PATHFINDING

    private Vector3 destination;

    private Vector3 current;

    private Vector3 goal;

    [SerializeField]
    private AStar astar;

    #endregion

    private Vector3 min, max;

    [SerializeField]
    private GearSocket[] gearSockets;

    [SerializeField]
    private Profession profession;

    private GameObject unusedSpell;

    private Spells aoeSpell;

    public int MyGold { get; set; }

    public List<IInteractable> MyInteractables
    {
        get
        {
            return interactables;
        }

        set
        {
            interactables = value;
        }
    }

    public Stat MyXp
    {
        get
        {
            return xpStat;
        }

        set
        {
            xpStat = value;
        }
    }

    public Stat MyMana
    {
        get
        {
            return mana;
        }

        set
        {
            mana = value;
        }
    }

    public List<Enemy> MyAttackers
    {
        get
        {
            return attackers;
        }

        set
        {
            attackers = value;
        }
    }

    protected override void Update()
    {
        GetInput();
        ClickToMove();
        // 初始化角色位置
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y),
            transform.position.z);

        if (unusedSpell != null)
        {
            Vector3 mouseScreenPostion = mainCam.ScreenToWorldPoint(Input.mousePosition);
            unusedSpell.transform.position = new Vector3(mouseScreenPostion.x, mouseScreenPostion.y, 0);

            float distance = Vector2.Distance(transform.position, mainCam.ScreenToWorldPoint(Input.mousePosition));

            if (distance >= aoeSpell.MyRange)
            {
                unusedSpell.GetComponent<AOESpell>().OutOfRange();
            }
            else
            {
                unusedSpell.GetComponent<AOESpell>().InRange();
            }

            if (Input.GetMouseButtonDown(0) && distance <= aoeSpell.MyRange)
            {
                AOESpell s = Instantiate(aoeSpell.MySpellPrefab, unusedSpell.transform.position, Quaternion.identity).GetComponent<AOESpell>();
                Destroy(unusedSpell);
                unusedSpell = null;
                s.Initialize(aoeSpell.MyDamage, aoeSpell.MyDuration);
            }
        }

        base.Update();
    }

    public void SetDefaultValues()
    {
        MyGold = 1000;
        health.Initialize(initHealth, initHealth);
        MyMana.Initialize(initMana, initMana);
        MyXp.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
        initPos = transform.parent.position;
    }

    /// <summary>
    /// 监听键盘输入
    /// </summary>
    private void GetInput()
    {
        Direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            health.MyCurrentValue -= 10;
            MyMana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(600);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            health.MyCurrentValue += 10;
            MyMana.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"])) //Moves up
        {
            exitIndex = 0;
            Direction += Vector2.up;
            minimapIcon.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"]))
        {
            exitIndex = 3;
            Direction += Vector2.left;
            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 90);
            }

        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;

            minimapIcon.eulerAngles = new Vector3(0, 0, 180);
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"]))
        {
            exitIndex = 1;
            Direction += Vector2.right;
            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 270);
            }

        }
        if (IsMoving)
        {
            StopAction();
            StopInit();
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
    private IEnumerator AttackRoutine(ICastable castable)
    {
        Transform currentTarget = MyTarget.MyHitbox;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if (currentTarget != null && InLineOfSight())
        {
            Spells newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);

            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.MyDamage, this,newSpell.MyDebuff);
        }

        StopAction();
    }

    private IEnumerator GatherRoutine(ICastable castable, List<Drop> items)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        LootWindow.MyInstance.CreatePages(items);
    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        profession.AdddItemsToInventory();
    }


    private IEnumerator ActionRoutine(ICastable castable)
    {
        SpellBook.MyInstance.Cast(castable);

        IsAttacking = true; // 标明攻击状态

        MyAnimator.SetBool("attack", IsAttacking);

        foreach (GearSocket g in gearSockets)
        {
            g.MyAnimator.SetBool("attack", IsAttacking);
        }

        yield return new WaitForSeconds(castable.MyCastTime);

        StopAction();

    }
    /// <summary>
    /// 释放技能
    /// </summary>
    public void CastSpell(Spells spell)
    {
        Block();

        if (!spell.NeedsTarget)
        {
            unusedSpell = Instantiate(spell.MySpellPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            unusedSpell.transform.position = new Vector3(unusedSpell.transform.position.x, unusedSpell.transform.position.y, 0);
            aoeSpell = spell;
        }

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive &&!IsAttacking && !IsMoving && InLineOfSight() && InRange(spell, MyTarget.transform.position)) // 检查当前是否能够攻击
        {
            MyInitRoutine = StartCoroutine(AttackRoutine(spell));
        }
    }

    private bool InRange(Spells spell, Vector2 targetPos)
    {

        if (Vector2.Distance(targetPos, transform.position) <= spell.MyRange)
        {
            return true;
        }
        MessageFeedManager.MyInstance.WriteMessage("超出距离啦!", Color.red);
        return false;
    }

    public void Gather(ICastable castable, List<Drop> items)
    {
        if (!IsAttacking)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(castable, items));
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

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
    public void StopAction()
    {
        // 停止技能释放
        SpellBook.MyInstance.StopCating();

        IsAttacking = false; // 修改攻击状态

        MyAnimator.SetBool("attack", IsAttacking); // 停止攻击动画

        foreach (GearSocket g in gearSockets)
        {
            g.MyAnimator.SetBool("attack", IsAttacking);
        }


        if (actionRoutine != null) 
        {
            StopCoroutine(actionRoutine);
        }
    }

    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
        }
    }

    public override void HandleLayers()
    {
        base.HandleLayers();

        if (IsMoving)
        {
            foreach (GearSocket g in gearSockets)
            {
                g.SetXAndY(Direction.x, Direction.y);
            }
        }
    }

    public override void ActivateLayer(string layerName)
    {
        base.ActivateLayer(layerName);

        foreach (GearSocket g in gearSockets)
        {
            g.ActivateLayer(layerName);
        }
    }


    public void GainXP(int xp)
    {
        MyXp.MyCurrentValue += xp;
        CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), SCTTYPE.XP, false);

        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }

    private IEnumerator Ding()
    {
        while (!MyXp.IsFull)
        {
            yield return null;
        }

        MyLevel++;
        ding.SetTrigger("Ding");
        levelText.text = MyLevel.ToString();
        MyXp.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyXp.MyMaxValue = Mathf.Floor(MyXp.MyMaxValue);
        MyXp.MyCurrentValue = MyXp.MyOverflow;
        MyXp.Reset();

        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }

    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void GetPath(Vector3 goal)
    {
        MyPath = astar.Algorithm(transform.position, goal);
        current = MyPath.Pop();
        destination = MyPath.Pop();
        this.goal = goal;
    }

    public IEnumerator Respawn()
    {
        MySpriteRenderer.enabled = false;
        yield return new WaitForSeconds(5f);
        health.Initialize(initHealth, initHealth);
        MyMana.Initialize(initMana, initMana);
        transform.parent.position = initPos;
        MySpriteRenderer.enabled = true;
        MyAnimator.SetTrigger("respawn");
        foreach (SpriteRenderer spriteRenderer in gearRenderers)
        {
            spriteRenderer.enabled = true;
        }
    }

    public void ClickToMove()
    {
        if (MyPath != null)
        {
            // 使角色朝目标移动
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, 2 * Time.deltaTime);

            Vector3Int dest = astar.MyTilemap.WorldToCell(destination);
            Vector3Int cur = astar.MyTilemap.WorldToCell(current);

            float distance = Vector2.Distance(destination, transform.parent.position);

            if (cur.y > dest.y)
            {
                Direction = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                Direction = Vector2.up;
            }
            if (cur.y == dest.y)
            {
                if (cur.x > dest.x)
                {
                    Direction = Vector2.left;
                }
                else if (cur.x < dest.x)
                {
                    Direction = Vector2.right;
                }
            }
            if (distance <= 0f)
            {
                if (MyPath.Count > 0)
                {
                    current = destination;
                    destination = MyPath.Pop();
                }
                else
                {
                    MyPath = null;
                }
            }
        }

    }

    public void HideGear()
    {
        foreach (SpriteRenderer spriteRenderer in gearRenderers)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" ||collision.tag== "Interactable")
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();

            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (MyInteractables.Count > 0)
            {
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                if (interactable != null)
                {
                    interactable.StopInteract();
                }

                MyInteractables.Remove(interactable);
            }

           
  
        }
    }
}
