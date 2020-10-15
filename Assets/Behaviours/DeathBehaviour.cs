using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour {

    private float timePassed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.transform.GetChild(0).gameObject);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timePassed += Time.deltaTime;

        if (timePassed >= 5)
        {
            Destroy(animator.gameObject);
        }
    }

}
