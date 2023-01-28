using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "AIController", menuName="InputController/AIController")]
    public class AIController : InputController
    {
        public override float RetrieveMovementInput()
        {
            return 1f;
        }

        public override bool RetrieveJumpInput()
        {
            return true;
        }
        
        public override bool RetrieveJumpInputHeld()
        {
            return false;
        }
    }
}