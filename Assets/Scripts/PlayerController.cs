using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    private Vector3 _direction;
    
    public KeyCode sprint = KeyCode.LeftShift;
    public Movement movement;

    public float jumpPower = 10f;
    public KeyCode jumpKey = KeyCode.Space;

    public float gravityMultiplier = 1f;
    private float _gravity = -9.8f;
    private float _Yveloctiy;

    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallCheckDistance = 0.7f;
    public float groundCheckDistance = 1.0f;
    public float wallRunGravityMulti = 2.5f;
    public float wallRunningSpeedMultiplier = 2;
    private bool _wallLeft;
    private bool _wallRight;
    private bool _isGroundNear;
    private RaycastHit _leftWallhit;
    private RaycastHit _rightWallhit;
    private RaycastHit _groundHit;

    public KeyCode aimKey = KeyCode.Mouse1;
    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(horizontal, 0f, vertical);
        
        // applies rotation, i probably should put it in a seperate function
        if(_direction.magnitude >= 0.1f){

            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // aligns the player with the movement
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // smoothes the alignment
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // honestly forgot lol
            _direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        
        ApplyJump();
        ApplyMovement();

        if(Input.GetKey(aimKey)){
            transform.forward = cam.transform.forward;
        }
    }
    private void ApplyMovement(){
        // Debug.Log(Input.GetKey(sprint));
		// float targetSpeed = Input.GetKey(sprint) ? movement.speed * movement.multiplier : movement.speed;
		// movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);
        
		movement.currentSpeed = Input.GetKey(sprint) ? movement.speed * movement.multiplier : movement.speed;

        // Debug.Log("isGrounded: " + controller.isGrounded);
        if(WallCheck() && !controller.isGrounded){
            WallRunning();
            WallJump();
        }
        else{
            ApplyGravity();
        }
        _direction.x *= movement.currentSpeed;
        _direction.z *= movement.currentSpeed;
        _direction.y = _Yveloctiy;
        
		controller.Move(_direction * Time.deltaTime);
	}
    private void ApplyGravity(){
        if(controller.isGrounded && _Yveloctiy < 0.0f){
            _Yveloctiy = -1.0f;
        }
        else{
            _Yveloctiy += _gravity * gravityMultiplier * Time.deltaTime;
        }
    }
    private void ApplyJump(){
        // Debug.Log("isgrounded?: " + controller.isGrounded + ", jumpKey: " + Input.GetKey(jumpKey));
        if(controller.isGrounded && Input.GetKey(jumpKey)){
            _Yveloctiy = jumpPower;
        }
    }
    private void WallJump(){
        // Debug.Log("isgrounded?: " + controller.isGrounded + ", jumpKey: " + Input.GetKey(jumpKey));
        if(!controller.isGrounded && WallCheck() && Input.GetKey(jumpKey)){
            _Yveloctiy = jumpPower;
        }
    }

    private bool WallCheck()
    {
        _wallRight = Physics.Raycast(transform.position, transform.right, out _rightWallhit, wallCheckDistance, whatIsWall);
        _wallLeft = Physics.Raycast(transform.position, -transform.right, out _leftWallhit, wallCheckDistance, whatIsWall);
        _isGroundNear = Physics.Raycast(transform.position, -transform.right, out _groundHit, groundCheckDistance, whatIsGround);

        return (_wallLeft || _wallRight) && !_isGroundNear;
    }
    private void WallRunning(){
        _Yveloctiy += _gravity * wallRunGravityMulti * Time.deltaTime;
        movement.currentSpeed *= wallRunningSpeedMultiplier;
    }
}


[Serializable]
public struct Movement
{
	public float speed;
	public float multiplier;
    public float acceleration;
	[HideInInspector] public float currentSpeed;
}