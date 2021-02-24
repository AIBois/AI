using UnityEngine;

namespace States
{
    public abstract class State : MonoBehaviour
    {
        private StateContext context;

        protected State(StateContext context)
        {
            this.context = context;
        }
        
        protected abstract void Act();

        protected void ChangeState(State state)
        {
            context.CurrentState = state;
        }

        void Update()
        {
            Act();
        }
    }
}
