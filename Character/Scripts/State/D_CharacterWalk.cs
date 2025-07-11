using _0.DucLib.Scripts.Machine;

namespace _0.DucTALib.Character.Scripts.State
{
    public class D_CharacterWalk : DTState
    {
        public D_CharacterAnimation animator;
        private bool isPlayAnim = false;
        public override void Init()
        {
            if(animator == null) animator = GetComponent<D_CharacterAnimation>();
        }

        public override void Enter()
        {
            base.Enter();
            if (!isPlayAnim)
            {
                isPlayAnim = true;
                animator.PlayRun();
            }
        }

        public override void Exit()
        {
            base.Exit();
            isPlayAnim = false;
        }
    }
}