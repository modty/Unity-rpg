using UnityEngine;

public class ActionBehaviour:StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Body.Instance.NotBusy();
    }
}
