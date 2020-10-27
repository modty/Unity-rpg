using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherLootTable : LootTable, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private Sprite gatherSprite;

    [SerializeField]
    private GameObject gatherIndicator;

    private void Start()
    {
        RollLoot();
    }

    protected override void RollLoot()
    {
        DroppedItems = new List<Drop>();

        foreach (Loot l in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= l.DropChance)
            {
                int itemCount = Random.Range(1, 6);

                for (int i = 0; i < itemCount; i++)
                {
//                    DroppedItems.Add(new Drop(Instantiate(l.ItemInGame), this));
                }

                spriteRenderer.sprite = gatherSprite;
                gatherIndicator.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Interact()
    {
        Player.Instance.Gather(SpellBook.Instance.GetSpell("Gather"), DroppedItems);
        LootWindow.Instance.Interactable = this;
    }

    public void StopInteract()
    {
        LootWindow.Instance.Interactable = null;

        if (DroppedItems.Count == 0)
        {
            spriteRenderer.sprite = defaultSprite;
            gameObject.SetActive(false);
            gatherIndicator.SetActive(false);
        }

        LootWindow.Instance.Close();
    }
}
