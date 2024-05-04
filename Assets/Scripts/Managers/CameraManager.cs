using UnityEngine;

[RequireComponent(typeof(CameraCore))]
public class CameraManager : Singleton<CameraManager> 
{
    [SerializeField] CameraCore _cameraCore;

    [Space, Header("States")]
    [SerializeField] CameraSinglePointState _singlePointState;

    Player _player;

    public bool IsActive = true;

    void Start()
    {
        _player = Player.Instance;
        _singlePointState.Target = _player.transform;
    }

    void Update()
    {
        if (!IsActive)
            return;
        SelectState();
        _cameraCore.CameraState.Execute();
    }

    void LateUpdate()
    {
        if (!IsActive)
            return;
        _cameraCore.CameraState.LateExecute();
    }

    void FixedUpdate()
    {
        if (!IsActive)
            return;
        _cameraCore.CameraState.FixedExecute();
    }

    void SelectState()
    {
        _cameraCore.StateMachine.SetState(_singlePointState);
    }
}