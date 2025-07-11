using UnityEngine;

namespace _0.DucTALib.Character.Scripts
{
    public class D_CharacterAnimation : CharacterAnimation
    {
        public Animator animator;

        public void PlayIdle()
        {
            animator.Play("Idle");
        }

        public void PlayWalk()
        {
            animator.Play("walk");
        }

        public void PlayRun()
        {
            animator.Play("Run");
        }
    }
}