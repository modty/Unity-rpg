using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    private static Player instance;

    public static Player Instance
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

    public List<IInteractable> Interactables
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

    public Stat Xp
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

    public Stat Mana
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

    public List<Enemy> Attackers
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

            if (distance >= aoeSpell.Range)
            {
                unusedSpell.GetComponent<AOESpell>().OutOfRange();
            }
            else
            {
                unusedSpell.GetComponent<AOESpell>().InRange();
            }

            if (Input.GetMouseButtonDown(0) && distance <= aoeSpell.Range)
            {
                AOESpell s = Instantiate(aoeSpell.SpellPrefab, unusedSpell.transform.position, Quaternion.identity).GetComponent<AOESpell>();
                Destroy(unusedSpell);
                unusedSpell = null;
                s.Initialize(aoeSpell.Damage, aoeSpell.Duration);
            }
        }

        base.Update();
    }

    public void SetDefaultValues()
    {
        MyGold = 1000;
        health.Initialize(initHealth, initHealth);
        Mana.Initialize(initMana, initMana);
        Xp.Initialize(0, Mathf.Floor(100 * Level * Mathf.Pow(Level, 0.5f)));
        levelText.text = Level.ToString();
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
            health.CurrentValue -= 10;
            Mana.CurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(600);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            health.CurrentValue += 10;
            Mana.CurrentValue += 10;
        }

        if (Input.GetKey(KeybindManager.Instance.Keybinds["UP"])) //Moves up
        {
            exitIndex = 0;
            Direction += Vector2.up;
            minimapIcon.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeybindManager.Instance.Keybinds["LEFT"]))
        {
            exitIndex = 3;
            Direction += Vector2.left;
            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 90);
            }

        }
        if (Input.GetKey(KeybindManager.Instance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;

            minimapIcon.eulerAngles = new Vector3(0, 0, 180);
        }
        if (Input.GetKey(KeybindManager.Instance.Keybinds["RIGHT"]))
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

        foreach (string action in KeybindManager.Instance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.Instance.ActionBinds[action]))
            {
                UIManager.Instance.ClickActionButton(action);

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
        Transform currentTarget = Target.Hitbox;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if (currentTarget != null && InLineOfSight())
        {
            Spells newSpell = SpellBook.Instance.GetSpell(castable.Title);

            SpellScript s = Instantiate(newSpell.SpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.Damage, this,newSpell.Debuff);
        }

        StopAction();
    }

    private IEnumerator GatherRoutine(ICastable castable, List<Drop> items)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        LootWindow.Instance.CreatePages(items);
    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        profession.AdddItemsToInventory();
    }


    private IEnumerator ActionRoutine(ICastable castable)
    {
        SpellBook.Instance.Cast(castable);

        IsAttacking = true; // 标明攻击状态

        Animator.SetBool("attack", IsAttacking);

        foreach (GearSocket g in gearSockets)
        {
            g.MyAnimator.SetBool("attack", IsAttacking);
        }

        yield return new WaitForSeconds(castable.CastTime);

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
            unusedSpell = Instantiate(spell.SpellPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            unusedSpell.transform.position = new Vector3(unusedSpell.transform.position.x, unusedSpell.transform.position.y, 0);
            aoeSpell = spell;
        }

        if (Target != null && Target.GetComponentInParent<Character>().IsAlive &&!IsAttacking && !IsMoving && InLineOfSight() && InRange(spell, Target.transform.position)) // 检查当前是否能够攻击
        {
            MyInitRoutine = StartCoroutine(AttackRoutine(spell));
        }
    }

    private bool InRange(Spells spell, Vector2 targetPos)
    {

        if (Vector2.Distance(targetPos, transform.position) <= spell.Range)
        {
            return true;
        }
        MessageFeedManager.Instance.WriteMessage("超出距离啦!", Color.red);
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
        if (Target != null)
        {
            // 目标方向
            Vector3 targetDirection = (Target.transform.position - transform.position).normalized;

            // 向目标方向发射射线
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.transform.position), 256);

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
        SpellBook.Instance.StopCating();

        IsAttacking = false; // 修改攻击状态

        Animator.SetBool("attack", IsAttacking); // 停止攻击动画

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
        Xp.CurrentValue += xp;
        CombatTextManager.Instance.CreateText(transform.position, xp.ToString(), SCTTYPE.XP, false);

        if (Xp.CurrentValue >= Xp.MaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!Attackers.Contains(enemy))
        {
            Attackers.Add(enemy);
        }
    }

    private IEnumerator Ding()
    {
        while (!Xp.IsFull)
        {
            yield return null;
        }

        Level++;
        ding.SetTrigger("Ding");
        levelText.text = Level.ToString();
        Xp.MaxValue = 100 * Level * Mathf.Pow(Level, 0.5f);
        Xp.MaxValue = Mathf.Floor(Xp.MaxValue);
        Xp.CurrentValue = Xp.MyOverflow;
        Xp.Reset();

        if (Xp.CurrentValue >= Xp.MaxValue)
        {
            StartCoroutine(Ding());
        }

    }

    public void UpdateLevel()
    {
        levelText.text = Level.ToString();
    }

    public void GetPath(Vector3 goal)
    {
        Path = astar.Algorithm(transform.position, goal);
        current = Path.Pop();
        destination = Path.Pop();
        this.goal = goal;
    }

    public IEnumerator Respawn()
    {
        SpriteRenderer.enabled = false;
        yield return new WaitForSeconds(5f);
        health.Initialize(initHealth, initHealth);
        Mana.Initialize(initMana, initMana);
        transform.parent.position = initPos;
        SpriteRenderer.enabled = true;
        Animator.SetTrigger("respawn");
        foreach (SpriteRenderer spriteRenderer in gearRenderers)
        {
            spriteRenderer.enabled = true;
        }
    }

    public void ClickToMove()
    {
        if (Path != null)
        {
            // 使角色朝目标移动
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, 2 * Time.deltaTime);

            Vector3Int dest = astar.Tilemap.WorldToCell(destination);
            Vector3Int cur = astar.Tilemap.WorldToCell(current);

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
                if (Path.Count > 0)
                {
                    current = destination;
                    destination = Path.Pop();
                }
                else
                {
                    Path = null;
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

            if (!Interactables.Contains(interactable))
            {
                Interactables.Add(interactable);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (Interactables.Count > 0)
            {
                IInteractable interactable = Interactables.Find(x => x == collision.GetComponent<IInteractable>());

                if (interactable != null)
                {
                    interactable.StopInteract();
                }

                Interactables.Remove(interactable);
            }

           
  
        }
    }
}
