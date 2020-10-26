using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour, ICastable
{

    [SerializeField]
    private CraftingMaterial[] materials;

    [SerializeField]
    private Item output;

    [SerializeField]
    private int outputCount;

    [SerializeField]
    private string description;

    [SerializeField]
    private Image highlight;

    [SerializeField]
    private float craftTime;

    [SerializeField]
    private Color barColor;


    public Item Output
    {
        get
        {
            return output;
        }
    }

    public int OutputCount
    {
        get
        {
            return outputCount;
        }

        set
        {
            outputCount = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public CraftingMaterial[] Materials
    {
        get
        {
            return materials;
        }
    }

    public string Title
    {
        get
        {
            return output.Title;
        }
    }
    public Sprite Icon
    {
        get
        {
            return output.Icon;
        }
    }
    public float CastTime
    {
        get
        {
            return craftTime;
        }
    }
    public Color BarColor
    {
        get
        {
            return barColor;
        }
    }


    void Start()
    {
        GetComponent<Text>().text = output.Title;
    }

    public void Select()
    {
        Color c = highlight.color;
        c.a = .3f;
        highlight.color = c;
    }

    public void Deselect()
    {
        Color c = highlight.color;
        c.a = 0f;
        highlight.color = c;

    }

}
