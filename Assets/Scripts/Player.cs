using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // 角色移动速度
    [SerializeField]
    private float speed;

    // 角色移动方向

    private Vector2 _direction;



	// 每帧调用
	void Update ()
    {
        // 获取键盘输入
        GetInput();

        //执行相应移动
        Move();
	}

    // 移动
    public void Move()
    {
        transform.Translate(speed * Time.deltaTime * _direction);
    }

    // 监听键盘输入
    private void GetInput()
    {
        _direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _direction += Vector2.right;
        }
    }
}
