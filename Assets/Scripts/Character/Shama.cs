using System;
using System.Collections.Generic;
using UnityEngine;

public class Shama:MonoBehaviour
{
    [SerializeField]
    private Animator selfAnimator;
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Spell1 = Animator.StringToHash("Spell1");
    private static readonly int Spell2 = Animator.StringToHash("Spell2");
    [SerializeField]
    private GameObject dollPrefab;
    /// <summary>
    /// 人偶(最大为5个)
    /// </summary>
    private List<ShamaDoll> dolls;
    private float endDistance=1.8f;
    private float startDistance = 0.8f;

    private void Awake()
    {
    }

    private void Start()
    {
        dolls=new List<ShamaDoll>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            selfAnimator.SetBool(Spell1,true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            selfAnimator.SetBool(Spell1,true);
            
            
        }
    }

    /// <summary>
    /// 创造木偶
    /// </summary>
    /// <param name="num"></param>
    /// <param name="angle"></param>
    public void CreateDoll(int angle)
    {
        if (dolls.Count <= 5)
        {
            ShamaDoll shamaDoll = Instantiate(dollPrefab).GetComponent<ShamaDoll>();
            dolls.Add(shamaDoll);
            var position = transform.position;
            Vector2 target=new Vector2((float) (endDistance*Math.Cos(angle)+position.x),(float) (endDistance*Math.Sin(angle)+position.y));
            Vector2 start=new Vector2((float) (startDistance*Math.Cos(angle)+position.x),(float) (startDistance*Math.Sin(angle)+position.y));
            shamaDoll.Position(start);
            shamaDoll.SpriteRenderer.enabled = true;
            shamaDoll.MoveTowards(target);
        }
        else
        {
            
        }
    }
}
