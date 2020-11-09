﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
 using Image = UnityEngine.UI.Image;

 public class Body : MonoBehaviour
 { 
    [SerializeField]
    private Animator selfAnimator;
    [SerializeField]
    private Animator sackAnimator;
    [SerializeField]
    private Animator weaponAnimator;
    [SerializeField]
    private GameObject weaponThrow;
    [SerializeField]
    private Transform weaponPosition;
    [SerializeField] 
    private BoxCollider2D boxCollider2D;
    [SerializeField]
    private Transform weaponSortPosition;
    [SerializeField]
    private SpriteRenderer weaponIcon;
    private Transform sackSortPosition;
    private ThrowWeaponController _throwWeaponController;

    private CharacterState _characterState;

    public CharacterState CharacterState
    {
        get => _characterState;
        set => _characterState = value;
    }

    private static Body instance;

    public static Body Instance => instance;
    private void Awake()
    {
        weaponSortPosition = weaponAnimator.gameObject.transform;
        sackSortPosition = sackAnimator.gameObject.transform;
        instance = this;
    }

    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    public bool attackBusy;
    public void DoAttackAnim(int attackType)
    {
        boxCollider2D.enabled = true;
        selfAnimator.SetInteger(Attack,attackType);
        weaponAnimator.SetInteger(Attack,attackType);
        sackAnimator.SetInteger(Attack,attackType);

    }

    public void DoMoveAnim(int x,int y,bool isMove)
    {
        if (x != -2 && y != -2)
        {
            selfAnimator.SetInteger(X,x);
            sackAnimator.SetInteger(X,x);
            weaponAnimator.SetInteger(X,x);
            selfAnimator.SetInteger(Y,y);
            sackAnimator.SetInteger(Y,y);
            weaponAnimator.SetInteger(Y,y);
        }
        selfAnimator.SetBool(Move,isMove);
        sackAnimator.SetBool(Move,isMove);
        weaponAnimator.SetBool(Move,isMove);
    }

    public void NotBusyPrepare()
    {
        boxCollider2D.enabled = false;
        selfAnimator.SetInteger(Attack,0);
        weaponAnimator.SetInteger(Attack,0);
        sackAnimator.SetInteger(Attack,0);
    }
    public void NotBusy()
    {
        attackBusy = false;
    }

    public void StartThrowWeapon()
    {
        if (weaponIcon.enabled)
        {
            weaponIcon.enabled = false;
        }

        if (!weaponThrow.activeSelf)
        {
            weaponThrow.SetActive(true);
        }
        
        if (_throwWeaponController==null)
        {
            _throwWeaponController = weaponThrow.GetComponent<ThrowWeaponController>();
        }
        _throwWeaponController.ThrowBack(_characterState);
    }

    public void stopThrowWeapon()
    {
        weaponIcon.enabled = true;
    }
    public void exchangeSortingLayer(int x,int y)
    {
        var position = sackSortPosition.position;
        var position1 = weaponSortPosition.position;
  
        if (y > 0&&x==0)
        {
            position=new Vector3(position.x,position.y,-2);
            position1=new Vector3(position1.x,position1.y,2);
            sackSortPosition.position = position;
            weaponSortPosition.position = position1;
        }
        else
        {
            position=new Vector3(position.x,position.y,2);
            position1=new Vector3(position1.x,position1.y,-2);
            sackSortPosition.position = position;
            weaponSortPosition.position = position1;
        }
        
    }

    public bool IsWeaponToTarget()
    {
        if (_throwWeaponController == null) return true;
        return _throwWeaponController.IsReady;
    }
}
