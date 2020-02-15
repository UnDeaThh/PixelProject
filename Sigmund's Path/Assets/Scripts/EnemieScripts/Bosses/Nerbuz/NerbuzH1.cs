using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerbuzH1 : StateMachineBehaviour
{
    private NerbuzBoss nerbuzBrain;
    private Vector2 movDir;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nerbuzBrain = animator.GetComponent<NerbuzBoss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //nerbuzBrain.AttackH1();   
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
