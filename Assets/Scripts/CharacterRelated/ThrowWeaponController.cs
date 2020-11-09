
//AXE

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowWeaponController : MonoBehaviour
{
    /// <summary>
    /// 决定旋转速度，回来时的速度是扔出的倍数
    /// </summary>
    private bool isThrowBack;
    /// <summary>
    /// 用于旋转扔出物品
    /// </summary>
    private bool isRotating;
    /// <summary>
    /// 扔出物品在扔出状态还是回归状态
    /// </summary>
    private bool backOrThrow;
    /// <summary>
    /// 目标点，鼠标点击位置
    /// </summary>
    private Vector2 targetPos;
    /// <summary>
    /// 武器回来后的动画
    /// </summary>
    public GameObject weaponReturnEffect;
    [SerializeField]
    private TrailRenderer tr;
    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    private CharacterState _characterState;
    private bool isReady=true;

    public bool IsReady
    {
        get => isReady;
        set => isReady = value;
    }

    public CharacterState CharacterState
    {
        get => _characterState;
        set => _characterState = value;
    }

    private void Update()
    { 
        SelfRotation();
        if (isThrowBack)
        {
            _rigidbody2D.position = Vector2.MoveTowards(_rigidbody2D.position, targetPos, Constants.WeaponMoveSpeed * Time.deltaTime);
        }
        else
        {
            _rigidbody2D.position = Vector2.MoveTowards(_rigidbody2D.position, _characterState.PlayerPosition, Constants.WeaponMoveSpeed * 3 * Time.deltaTime);
        }
        ReachAtMousePosition();
        ReachAtPlayerPosition();
    }

    private void ReachAtMousePosition()
    {
        if (!backOrThrow&&Vector2.Distance(targetPos, _rigidbody2D.position) <= 0.01f)
        {
            isRotating = false;
            backOrThrow = true;
            tr.enabled = false;
            IsReady = true;
            _rigidbody2D.position = targetPos;
        }
    }

    private void ReachAtPlayerPosition()
    {
        if (backOrThrow&&Vector2.Distance(_rigidbody2D.position, _characterState.PlayerPosition) <= 0.01f)
        {
            isRotating = false;
            backOrThrow = false;
            tr.enabled = false;
            IsReady = true;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            Body.Instance.stopThrowWeapon();
            Instantiate(weaponReturnEffect, _characterState.PlayerPosition, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    private void SelfRotation()
    {
        if(isRotating)
        {
            transform.Rotate(0, 0, Constants.WeaponThrowRotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }

    private void ThrowWeapon()
    {
        targetPos = _characterState.MousePosition;
        isRotating = true;
        isThrowBack = true;
    }
    private void BackWeapon()
    {
        isRotating = true;
        isThrowBack = false;
    }

    /// <summary>
    /// 外部调用，每一次点击，调用一次，内部决定是回来还是扔出
    /// </summary>
    public void ThrowBack(CharacterState characterState)
    {
        if (isReady)
        {
            IsReady = false;
            _characterState = characterState;
            tr.enabled = true;
            if (backOrThrow) BackWeapon();
            else ThrowWeapon();
        }
    }
}
