using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour
{
    public Animator MyAnimator { get; set; }

    protected SpriteRenderer spriteRenderer;

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);

        MyAnimator.runtimeAnimatorController = animatorOverrideController;
        
    }

    public virtual void SetXAndY(float x, float y)
    {
        // 设置正确的动画参数
        MyAnimator.SetFloat("x", x);
        MyAnimator.SetFloat("y", y);
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public void Equip(AnimationClip[] animations)
    {
        spriteRenderer.color = Color.white;
        animatorOverrideController["Wizard_Attack_Back"] = animations[0];
        animatorOverrideController["Wizard_Attack_Front"] = animations[1];
        animatorOverrideController["Wizard_Attack_Left"] = animations[2];
        animatorOverrideController["Wizard_Attack_Right"] = animations[3];

        animatorOverrideController["Wizard_Idle_Back"] = animations[4];
        animatorOverrideController["Wizard_Idle_Front"] = animations[5];
        animatorOverrideController["Wizard_Idle_Left"] = animations[6];
        animatorOverrideController["Wizard_Idle_Right"] = animations[7];

        animatorOverrideController["Wizard_Walk_Back"] = animations[8];
        animatorOverrideController["Wizard_Walk_Front"] = animations[9];
        animatorOverrideController["Wizard_Walk_Left"] = animations[10];
        animatorOverrideController["Wizard_Walk_Right"] = animations[11];
    }

    public void Dequip()
    {
        animatorOverrideController["Wizard_Attack_Back"] = null;
        animatorOverrideController["Wizard_Attack_Front"] = null;
        animatorOverrideController["Wizard_Attack_Left"] = null;
        animatorOverrideController["Wizard_Attack_Right"] = null;

        animatorOverrideController["Wizard_Idle_Back"] = null;
        animatorOverrideController["Wizard_Idle_Front"] = null;
        animatorOverrideController["Wizard_Idle_Left"] = null;
        animatorOverrideController["Wizard_Idle_Right"] = null;

        animatorOverrideController["Wizard_Walk_Back"] = null;
        animatorOverrideController["Wizard_Walk_Front"] = null;
        animatorOverrideController["Wizard_Walk_Left"] = null;
        animatorOverrideController["Wizard_Walk_Right"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }
}
