using System;
using UnityEngine;
using UnityEngine.UI;

public class LOLPointMesScript:MonoBehaviour
{
    private static LOLPointMesScript instance;
    public static LOLPointMesScript Instance => instance;
    [SerializeField] private Text mes;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (mes.enabled)
        {
            transform.position = Input.mousePosition+new Vector3(-10,10);
        }
    }

    public void ShowMes(string text)
    {
        mes.text = text;
        transform.position = Input.mousePosition+new Vector3(-10,10);
        mes.enabled = true;
    }

    public void Close()
    {
        mes.enabled = false;
    }
}
