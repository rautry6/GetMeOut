using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMovementInput();
    public abstract bool RetrieveJumpInput();
    public abstract bool RetrieveJumpInputHeld();
}
