using UnityEngine;

namespace TechArtProject.Animations
{
    public class DisableGameObjectOnExit : StateMachineBehaviour
    {
        public bool DisableOnEnter = false;
        public bool DisableOnExit = true;

        // Called when we enter the state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (DisableOnEnter)
                animator.gameObject.SetActive(false);
        }

        // Called when we exit the state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (DisableOnExit)
                animator.gameObject.SetActive(false);
        }
    }
}