using UnityEngine;

//62

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();
public class NPC : Character
{
    public event HealthChanged healthChanged;
    
    public event CharacterRemoved characterRemoved;
    
    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }
    
    public virtual void DeSelect()
    {
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);

        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
    }

    public virtual Transform Select()
    {
        return hitBox;
    }
    
    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    
    }
    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }
    public virtual void Interact()
    {
        // 没有被重载的话
        Debug.Log("This will open a dialogue with the NPC");
    }
}