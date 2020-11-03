using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private static UIScript instance;
    public static UIScript Instance => instance;
    [SerializeField]private GameObject pointMes;
    [SerializeField]private GameObject stat;
    [SerializeField]private GameObject inventory;
    [SerializeField]private GameObject bagBar;
    [SerializeField]private GameObject targetFrame;
    [SerializeField]private GameObject selectPlane;
    [SerializeField]private Camera mainCam;
    [SerializeField] private GameObject mesPlane;

    public GameObject MesPlane
    {
        get => mesPlane;
        set => mesPlane = value;
    }

    public static UIScript Instance1
    {
        get => instance;
        set => instance = value;
    }

    public GameObject PointMes
    {
        get => pointMes;
        set => pointMes = value;
    }

    public GameObject Stat
    {
        get => stat;
        set => stat = value;
    }

    public GameObject Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    public GameObject BagBar
    {
        get => bagBar;
        set => bagBar = value;
    }

    public GameObject TargetFrame
    {
        get => targetFrame;
        set => targetFrame = value;
    }

    public GameObject SelectPlane
    {
        get => selectPlane;
        set => selectPlane = value;
    }

    public Camera MainCam
    {
        get => mainCam;
        set => mainCam = value;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            selectPlane.transform.position = Input.mousePosition;
            selectPlane.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            selectPlane.SetActive(false);
        }
    }
}
