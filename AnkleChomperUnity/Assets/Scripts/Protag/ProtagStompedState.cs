using Cysharp.Threading.Tasks;
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

        [SerializeField]
        private UnityEvent _onStompedPopup;

        [SerializeField]
        private float _stompedPopupDelay;

        public override void EnterState()
        {
            Animator.Play("kid_armature|Stunned", 0, 0f);
            _ikRig.weight = 0f;
            _onStomped?.Invoke();
            StompedPopup().Forget();
        }

        private async UniTaskVoid StompedPopup()
        {
            await UniTask.Delay((int)(_stompedPopupDelay * 1000));
            _onStompedPopup?.Invoke();
        }

        public override void OnExitState()
        {
        }

        public override bool CanEnterState()
        {
            return _controller.CurrentState != this;
        }

        [Button("GetStomped")]
        public void GetStomped()
        {
            _controller.SwitchToState(this);
        }
    }
}