using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float speed = 10f;
    public Rigidbody rb;
    private CharacterController controller;
    private bool inFlyMode;

    void Start () {
      rb = GetComponentInParent<Rigidbody>();
      controller = GetComponent<CharacterController>();
      FlyModeOn();
    }

    void FixedUpdate () {
      CheckState();
      if (!inFlyMode) return;
      Movement();
      LookRotation();
    }

    private void FlyModeOn()
    {
      inFlyMode = true;
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    private void FlyModeOff()
    {
      controller.Move(new Vector3(0, 0, 0));
      inFlyMode = false;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    private void CheckState()
    {
      if (Input.GetKey(KeyCode.Mouse0)) FlyModeOff();
      if (Input.GetKey(KeyCode.Mouse1)) FlyModeOn();
    }

    private void LookRotation()
    {
      float mouseX = Input.GetAxis ("Mouse X") * mouseSensitivity * Time.deltaTime;
      float mouseY = Input.GetAxis ("Mouse Y") * mouseSensitivity * Time.deltaTime;

      xRotation -= mouseY;
      xRotation = Mathf.Clamp(xRotation, -90f, 90f);

      transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
      rb.transform.Rotate(Vector3.up * mouseX);
    }

    private void Movement()
    {
      float x = Input.GetAxis("Horizontal");
      float z = Input.GetAxis("Vertical");

      Vector3 move = transform.right * x + transform.forward * z;

      controller.Move(move * speed * Time.deltaTime);
    }
}
