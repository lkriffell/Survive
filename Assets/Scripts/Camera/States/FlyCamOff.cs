using UnityEngine;

public class FlyCamOff : IState
{
  private readonly CameraController _cameraController;
    public FlyCamOff(CameraController cameraController)
    {
      _cameraController = cameraController;
    }

    public void OnEnter() 
    { 
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    public void Tick() 
    { 
    }

    public void OnExit() 
    { 
    }
}