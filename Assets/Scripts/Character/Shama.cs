using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    private float playerDistance=3f;
    private float startDistance = 0.8f;
    private bool isReady;
    private List<bool> readyArray;
    private void Awake()
    {
        
    }
    private void Start()
    {
        dolls=new List<ShamaDoll>();
        readyArray=new List<bool>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            if (dolls.Count > 0)
            {
                // 计算角度
                int angle = (360 / dolls.Count)%360;
                for (int i = 0; i < dolls.Count; i++)
                {
                    Vector2 playerPosition = Player.Instance.ControlledChaState.PlayerPosition;
                    Vector2 vector2=new Vector2(
                        (float) (playerDistance*Math.Cos((angle*i+90)*(Math.PI*2/360))+playerPosition.x),
                        (float) (playerDistance*Math.Sin((angle*i+90)*(Math.PI*2/360))+playerPosition.y)
                    );
                    dolls[i].MoveTowards(vector2,6,readyArray,this);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            foreach (var doll in dolls)
            {
                doll.Active(false);
            }
        }
    }

    public void StopAction()
    {
        selfAnimator.SetBool(Spell1,false);
        selfAnimator.SetBool(Spell2,false);
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
        if (dolls.Count < 5)
        {
            ShamaDoll shamaDoll = Instantiate(dollPrefab).GetComponent<ShamaDoll>();
            dolls.Add(shamaDoll);
            var position = transform.position;
            Vector2 target=new Vector2((float) (endDistance*Math.Cos(angle*(Math.PI*2/360))+position.x),(float) (endDistance*Math.Sin(angle*(Math.PI*2/360))+position.y));
            Vector2 start=new Vector2((float) (startDistance*Math.Cos(angle*(Math.PI*2/360))+position.x),(float) (startDistance*Math.Sin(angle*(Math.PI*2/360))+position.y));
            shamaDoll.Position(start);
            shamaDoll.MoveTowards(target,1.5f);
        }
    }

    public void TryDollActive()
    {
        if (readyArray.Count==dolls.Count)
        {
            bool ready=true;
            foreach (var rea in readyArray)
            {
                if (!rea)
                {
                    ready = false;
                    break;
                }
            }
            if (ready)
            {
                foreach (var doll in dolls)
                {
                    doll.Active(true);
                }
            }
        }
    }
}
