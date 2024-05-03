using UnityEngine;

public abstract class CameraState : State
{
    public CameraCore CameraCore { get { return (CameraCore)Core;}}

    public abstract void LateExecute();
}