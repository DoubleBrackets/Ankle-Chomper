using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

namespace Protag
{
    public class ProtagStompedState : ProtagState
    {
        [SerializeField]
        private Rig _ikRig;

        [SerializeField]
        private UnityEvent _onStomped;

        public override void EnterState()
        {
            Animator.Play("kid_armature|Stunned", 0, 0f);
            _ikRig.weight = 0f;
            _onStomped?.Invoke();
        }

        public override void OnExitState()
        {
        }

        public override bool CanEnterState()
        {
            return _controller.CurrentState != this;
        }

        [Button("GetStomped")]
        private void GetStomped()
        {
            _controller.SwitchToState(this);
        }
    }
}