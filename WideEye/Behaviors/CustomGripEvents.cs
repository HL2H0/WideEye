using Il2CppSLZ.Marrow;
using UnityEngine;
using UnityEngine.Events;

namespace WideEye.Behaviors
{
    public class CustomGripEvents : MonoBehaviour
    {
        public Grip grip;
    
        public UnityEvent onIndex;

        public UnityEvent onMenuButtonDown;
        private void Update()
        {
            if (!grip.GetController(out var controller)) return;
            if (controller.GetPrimaryInteractionButton())
                onIndex.Invoke();
            if(controller.GetMenuButtonDown())
                onMenuButtonDown.Invoke();
        }
    }

}
