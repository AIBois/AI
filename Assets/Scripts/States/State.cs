using UnityEngine;

namespace States
{
    public abstract class State : MonoBehaviour
    {
        protected abstract void Act();

        void Update()
        {
            Act();
        }
    }
}
