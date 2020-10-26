using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow instance;

    public static LootWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LootWindow>();
            }
            return instance;
        }
    }


    [SerializeField]
    private LootButton[] lootButtons;

    private CanvasGroup canvasGroup;

    private List<List<Drop>> pages = new List<List<Drop>>();

    private List<Drop> droppedLoot = new List<Drop>();

    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextBtn, previousBtn;

    public IInteractable Interactable { get; set; }

    [SerializeField]
    private Item[] items;

    public bool IsOpen
    {
        get { return canvasGroup.alpha > 0; }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void CreatePages(List<Drop> items)
    {
        if (!IsOpen)
        {
            List<Drop> page = new List<Drop>();

            droppedLoot = items;

            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);

                if (page.Count == 4 || i == items.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Drop>();
                }
            }

            AddLoot();

            Open();
        }


    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            // 处理页数
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;

            // 判断是否有前一页、后一页
            previousBtn.SetActive(pageIndex > 0);
            nextBtn.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);

            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    // 设置掉落物图标
                    lootButtons[i].Icon.sprite = pages[pageIndex][i].Item.Icon;

                    lootButtons[i].Loot = pages[pageIndex][i].Item;

                    // 确认战利品按钮可用
                    lootButtons[i].gameObject.SetActive(true);

                    string title = string.Format("<color={0}>{1}</color>", "#00ff00ff", pages[pageIndex][i].Item.Title);

                    // 设置战利品名称
                    lootButtons[i].Title.text = title;
                }

            }
        }



    }

    public void ClearButtons()
    {
        foreach (LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        // 如果还有更多的页数
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        // 如果有前一夜
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot)
    {

        Drop drop = pages[pageIndex].Find(x => x.Item == loot);

        pages[pageIndex].Remove(drop);

        drop.Remove();

        if (pages[pageIndex].Count == 0)
        {
            // 移出空白页
            pages.Remove(pages[pageIndex]);

            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            AddLoot();
        }
    }

    public void Close()
    {
        pageIndex = 0;
        pages.Clear();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ClearButtons();

        if (Interactable != null)
        {
            Interactable.StopInteract();
        }

        Interactable = null;
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
