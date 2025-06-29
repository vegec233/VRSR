using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class E_Behaviour : StateMachineBehaviour
{
    [SerializeField]
    private float timeToSwitchTalk;
    [SerializeField]
    private int numAnimations;

    private bool isTalking2;
    private float talk1Time;

    private int talkAnimation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetTalk();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isTalking2)
        {
            talk1Time += Time.deltaTime;

            if (talk1Time > timeToSwitchTalk && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isTalking2 = true;
                talkAnimation = Random.Range(0, numAnimations);

                //animator.SetFloat("TalkAnimation", talkAnimation);
                Debug.Log(talkAnimation);

            }

        }

        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetTalk();
        }
        
        animator.SetFloat("TalkAnimation", talkAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetTalk()
    {
        isTalking2 = false;
        talk1Time = 0;

        talkAnimation = 0;

        //animator.SetFloat("TalkAnimation", 0);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
