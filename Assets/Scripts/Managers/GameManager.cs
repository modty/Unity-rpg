using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour {

    public event KillConfirmed killConfirmedEvent;

    private Camera mainCamera;

    private static GameManager instance;

    /// <summary>
    /// 角色引用
    /// </summary>
    [SerializeField]
    private Player player;

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;

    private Enemy currentTarget;
    private int targetIndex;

    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();


    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }

    }

    public HashSet<Vector3Int> Blocked
    {
        get
        {
            return blocked;
        }

        set
        {
            blocked = value;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update ()
    {
        ClickTarget();

        NextTarget();
	}

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // 判断鼠标左键是否按下，判断是否点击到UI上
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,Mathf.Infinity,512);

            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                DeSelectTarget();

                SelectTarget(hit.collider.GetComponent<Enemy>());
            }
            else
            {
                UIManager.Instance.HideTargetFrame();

                DeSelectTarget();

                currentTarget = null;
                player.Target = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();
                if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.Interactables.Contains(entity))
                {
                    entity.Interact();
                }
            }
            else
            {
                hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

                if (hit.collider != null)
                {
                    player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                }
            }
        }
   
    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeSelectTarget();

            if (Player.Instance.Attackers.Count > 0)
            {
                if (targetIndex < Player.Instance.Attackers.Count)
                {
                    SelectTarget(Player.Instance.Attackers[targetIndex]);
                    targetIndex++;
                    if (targetIndex >= Player.Instance.Attackers.Count)
                    {
                        targetIndex = 0;
                    }

                }
                else
                {
                    targetIndex = 0;
                }
        
            }
        }

    }

    private void DeSelectTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.DeSelect();
        }

    }

    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.Target = currentTarget.Select();
        UIManager.Instance.ShowTargetFrame(currentTarget);


    }

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent !=null)
        {
            killConfirmedEvent(character);
        }
    }
}
