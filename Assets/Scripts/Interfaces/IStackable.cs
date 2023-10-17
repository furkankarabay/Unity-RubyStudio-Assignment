using UnityEngine;

namespace RubyGame.Interfaces
{
    public interface IStackable
    {
        public void QueueInStack(Transform parent, Vector3 position);
        public void LeaveFromStack();

    }
}
