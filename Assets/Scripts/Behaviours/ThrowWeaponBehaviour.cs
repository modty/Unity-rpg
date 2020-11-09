
using UnityEngine;

public class ThrowWeaponBehaviour:StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Body.Instance.StartThrowWeapon();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Body.Instance.NotBusy();
    }
}
