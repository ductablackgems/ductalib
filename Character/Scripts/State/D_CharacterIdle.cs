using _0.DucLib.Scripts.Machine;
using Sirenix.OdinInspector;

namespace _0.DucTALib.Character.Scripts.State
{
    public class D_CharacterIdle : DTState
    {
        [ReadOnly] public D_CharacterAnimation animator;
        private bool isPlayAnim = false;

        public override void Init()
        {
            if (animator == null) animator = GetComponent<D_CharacterAnimation>();
        }

        public override void Enter()
        {
            base.Enter();
            if (!isPlayAnim)
            {
                isPlayAnim = true;
                animator.PlayIdle();
            }
        }

        public override void Exit()
        {
            base.Exit();
            isPlayAnim = false;
        }
    }
}