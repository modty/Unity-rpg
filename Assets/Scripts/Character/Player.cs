using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private static Player instance;

    public static Player Instance => instance;
    
    [SerializeField]
    private Body myBody;
    private int[] moveDir;
    private int[] lastMoveDir={0,0};
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    bool move;
    private bool horizontalChange;
    private bool verticalChange=false;
    private CharacterState _characterState;
    [SerializeField]
    private Rigidbody2D rigidbody2D;
    private Rigidbody2D rb;
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    public CharacterState CharacterState
    {
        get => _characterState;
        set
        {
            _characterState = value;
            _characterState.PlayerPosition = transform.position;
            myBody.CharacterState = _characterState;
        }
    }

    public void Update()
    {
        HandleAction();
        MoveDir();
        HandleMove();
        Jump();
    }

    private bool attackChange;
    private void HandleAction()
    {
        if (myBody.attackBusy||_characterState.IsJump) return;
        int attackType = 0;
        if (Input.GetKeyDown(KeyCode.Mouse0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            attackType = 1;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            attackType = 3;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _characterState.IsJump = true;//跳跃状态赋值为true
            ReadyJump();//执行准备跳跃方法
            return;
        }
        if (attackType != 0)
        {
            if (move)
            {
                myBody.DoMoveAnim(-2,-2,false);
            }
            myBody.attackBusy = true;
            moveDir=MousePositionToDirection();
            myBody.exchangeSortingLayer(moveDir[0],moveDir[1]);
            myBody.DoMoveAnim(moveDir[0],moveDir[1],false);
            if (attackType <= 2)
            {
                attackChange = !attackChange;

                if (attackChange)
                {
                    myBody.DoAttackAnim(1);
                }
                else
                {
                    myBody.DoAttackAnim(2);
                }
            }else if (attackType == 3&&myBody.IsWeaponToTarget())
            {
                myBody.DoAttackAnim(3);
            }
            else
            {
                myBody.attackBusy = false;
            }
        }
        
    }

    private Vector2 MousePosition()
    {
        Vector2 vector2 = rb.position;
        Vector2 vector2Temp=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _characterState.MousePosition = vector2Temp;
        vector2 = vector2Temp - vector2;
        return vector2;
    }

    private int[] MousePositionToDirection()
    {
        Vector2 vector2 = MousePosition();
        int[] arrTemp;
        if (vector2.x > 0)
        {
            if (vector2.x >= Math.Abs(vector2.y)) arrTemp = new[] {1, 0};
            else
            {
                if (vector2.y > 0) arrTemp = new[] {0, 1};
                else arrTemp = new[] {0, -1};
            }
        }
        else
        {
            if (Math.Abs(vector2.x) >= Math.Abs(vector2.y)) arrTemp = new[] {-1, 0};
            else
            {
                if (vector2.y > 0) arrTemp = new[] {0, 1};
                else arrTemp = new[] {0, -1};
            }
        }
        return arrTemp;
    }
    private void HandleMove()
    {
        if (myBody.attackBusy) return;
        int[] moveAnimArr={-2,-2};
        move = false;
        bool isIdle = moveDir[0] == 0 && moveDir[1] == 0;
        if (!isIdle) {
            _characterState.PlayerPosition = transform.position;
            if (moveDir[0]!=0)
            {
                moveAnimArr=new []{moveDir[0],0};
            }
            else
            {
                moveAnimArr=new []{0,moveDir[1]};
            }
            if (lastMoveDir[0]==moveDir[0]&&lastMoveDir[1]==moveDir[1])
            {
                move = true;
            }
            rb.position = rb.position + Constants.ChaMoveSpeed * Time.deltaTime * _characterState.MoveVec;
            lastMoveDir = moveDir;
            myBody.exchangeSortingLayer(moveDir[0],moveDir[1]);
        }
        myBody.DoMoveAnim(moveAnimArr[0],moveAnimArr[1],move);
    }


    private void MoveDir()
    {
        if(myBody.attackBusy) return;
        int moveX = 0;
        int moveY = 0;
        if (horizontalChange)
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                moveX = -1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                if (moveX == 0)
                {
                    horizontalChange = false;
                }
                moveX = 1;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                moveX = 1;
            }
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                if (moveX == 0)
                {
                    horizontalChange = true;
                }
                moveX = -1;
            }
        }
        if (verticalChange)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                moveY = 1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                if (moveY == 0)
                {
                    verticalChange = false;
                }
                moveY = -1;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                moveY = -1;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                if (moveY == 0)
                {
                    verticalChange = true;
                }
                moveY = 1;
            }
        }
        moveDir =  new []{moveX,moveY};
        _characterState.MoveVec=new Vector3(moveX,moveY);
    }
    
    public float jumpHeight =2.5f;//跳跃高度
    public float aSpeed = -9.8f;//重力加速度
    private Vector2 direction;//移动方向（父物体，xy轴平面）
    private float velocity_Y;//跳跃速度(子物体)

    private Animator ani;//动画控制器(子物体)
    private Rigidbody2D rig;//刚体组件(父物体)

    public Transform childTransform;//Transform组件(子物体)

    void Jump()
    {
        velocity_Y += aSpeed * Time.fixedDeltaTime;//重力模拟(子物体垂直速度始终受重力加速度影响)
        //判断子物体是在下落状态(velocity小于零)并且距离父物体小于等于0.05
        if (childTransform.position.y <= rb.position.y + 0.05f && velocity_Y < 0)
        {
            //如果满足
            velocity_Y = 0;// 子物体垂直速度清零
            childTransform.position = rb.position;//子物体position与父物体对齐
            gameObject.layer = 9;

            _characterState.IsJump = false;//则将跳跃状态设置为false，等待下一次跳跃
        }
        childTransform.Translate(Time.fixedDeltaTime * new Vector3(0, velocity_Y));//子物体按照速度移动
    }

    void ReadyJump()
    {
        gameObject.layer = 10;
        velocity_Y = Mathf.Sqrt(jumpHeight * -2f * aSpeed);
    }

    public long Uid()
    {
//        return _characterState.Item.uid;
        return 123;
    }
}
