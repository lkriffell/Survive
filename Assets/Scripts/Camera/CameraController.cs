using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    public float speed = 10f;
    public Rigidbody rb;
    public CharacterController controller;
    private StateMachine _stateMachine;
    private bool inFlyMode = true;

    void Start () {
      rb = GetComponentInParent<Rigidbody>();
      controller = GetComponent<CharacterController>();

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      _stateMachine = new StateMachine();

      var flyCamOn = new FlyCamOn(this);
      var flyCamOff = new FlyCamOff(this);

      At(flyCamOn, flyCamOff, FlyCamOn());
      At(flyCamOff, flyCamOn, FlyCamOff());

      _stateMachine.SetState(flyCamOn);

      void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

      Func<bool> FlyCamOn() => () => Input.GetKey(KeyCode.Mouse0);
      Func<bool> FlyCamOff() => () => Input.GetKey(KeyCode.Mouse1);
    }

    void FixedUpdate () {
      _stateMachine.Tick();
      Movement();
    }

    private void Movement()
    {
      float x = Input.GetAxis("Horizontal");
      float z = Input.GetAxis("Vertical");

      Vector3 move = transform.right * x + transform.forward * z;

      controller.Move(move * speed * Time.deltaTime);
    }
}
