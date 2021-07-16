using UnityEngine;

public class FlyCamOn : IState
{
    private readonly CameraController _cameraController;

    public FlyCamOn(CameraController cameraController) 
    {
      _cameraController = cameraController;
    }
    public void OnEnter() 
    { 
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
    public void Tick() 
    { 
      LookRotation();
    }

    public void OnExit() { 
    }

    private void LookRotation()
    {
      float mouseX = Input.GetAxis ("Mouse X") * _cameraController.mouseSensitivity * Time.deltaTime;
      float mouseY = Input.GetAxis ("Mouse Y") * _cameraController.mouseSensitivity * Time.deltaTime;

      _cameraController.xRotation -= mouseY;
      _cameraController.xRotation = Mathf.Clamp(_cameraController.xRotation, -90f, 90f);

      _cameraController.transform.localRotation = Quaternion.Euler(_cameraController.xRotation, 0f, 0f);
      _cameraController.rb.transform.Rotate(Vector3.up * mouseX);
    }

}