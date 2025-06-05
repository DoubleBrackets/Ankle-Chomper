using UnityEngine;

namespace Protag
{
    public abstract class ProtagState : MonoBehaviour
    {
        protected Animator Animator => _controller.Animator;
        protected ProtagController _controller;
        public abstract void EnterState();

        public abstract void OnExitState();

        public abstract bool CanEnterState();

        public void Initialize(ProtagController controller)
        {
            _controller = controller;
        }
    }
}