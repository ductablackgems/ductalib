using UnityEngine;

namespace _0.DucLib.Scripts.Machine
{
    public class StateMachine : MonoBehaviour
    {
        protected DTState _currentState;

        protected bool _inTransition;

        public virtual DTState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                Transition(value);
            }
        }

        public virtual T GetState<T>() where T : DTState
        {
            T val = GetComponent<T>();
            if ((Object)val == (Object)null)
            {
                val = base.gameObject.AddComponent<T>();
                val.Init();
            }
            return val;
        }
        public bool IsInState<T>() where T : DTState
        {
            return _currentState is T;
        }
        public virtual void ChangeState<T>() where T : DTState
        {
            CurrentState = GetState<T>();
        }

        protected virtual void Transition(DTState value)
        {
            if (!(_currentState == value) && !_inTransition)
            {
                _inTransition = true;
                if (_currentState != null)
                {
                    _currentState.Exit();
                }
                _currentState = value;
                if (_currentState != null)
                {
                    _currentState.Enter();
                }
                _inTransition = false;
            }
        }
    }
}