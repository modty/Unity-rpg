using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject stat;

    [SerializeField] private GameObject Inventory;
    [SerializeField]private GameObject bagBar;
    [SerializeField]private GameObject targetFrame;
    [SerializeField]private GameObject selectPlane;
    [SerializeField]private Camera mainCam;

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
