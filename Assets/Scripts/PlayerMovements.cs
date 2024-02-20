using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawArrow;
using static EasingFunction;

public class PlayerMovements : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)]
	private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
	private float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 100f)]
	private float jumpHeight = 2f;

    [SerializeField, Range(0f, 100f)]
    private float gravity = 90f;

    [SerializeField, Range(0f, 1f)]
    float coyoteTime = 0.2f;

    [SerializeField, Range(0f, 1f)]
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [SerializeField, Range(0, 2)]
    private int JumpCount;
    private bool desiredJump;
    private float lastOnGroundTime;
    private Vector3 velocity, desiredVelocity;
    private Rigidbody body;
    [SerializeField, Range(0f, 90f)]
	float maxGroundAngle = 25f;
    float minGroundDotProduct;
    Vector3 contactNormal;
    public Transform orientation;

	void OnValidate () {
		minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
	}
    void Awake () {
		body = GetComponent<Rigidbody>();
        OnValidate();
	}

    void Update () {
        lastOnGroundTime -= Time.deltaTime;
        getInputs();
	}

    void FixedUpdate()
    {
        Run();
    }

    void getInputs(){
        Vector3 playerInput;
		float horDir = Input.GetAxis("Horizontal");
        float verDir = Input.GetAxis("Vertical");

        playerInput = horDir * orientation.right + verDir * orientation.forward;

        playerInput.y = 0.0f;

        playerInput = playerInput.normalized;

        desiredVelocity = playerInput * maxSpeed;

        desiredJump |= Input.GetButtonDown("Jump");
    }


    void Run()
    {
        velocity = body.velocity;
        AdjustVelocity();

        if (desiredJump)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if(jumpBufferCounter>0f)
        {
            desiredJump = false;
            JumpCount--;
		    Jump();
        }

		body.velocity = velocity;
        lastOnGroundTime = 0.0f;
        GetComponent<Rigidbody>().AddForce(Vector3.down * gravity * GetComponent<Rigidbody>().mass);
    }

    void Jump()
    {
        if(lastOnGroundTime > 0f || JumpCount>0)
        {
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);
			if (alignedSpeed > 0f) {
				jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
			}
			velocity += contactNormal * jumpSpeed;
            jumpBufferCounter = 0f;
        }
    }

    void AdjustVelocity () {
		Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
		float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = (lastOnGroundTime>0.0f) ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        EasingFunction.Ease ease = EasingFunction.Ease.EaseInOutCubic;
        EasingFunction.Function func = GetEasingFunction(ease);

        float newX = func(currentX, desiredVelocity.x, maxSpeedChange);
		float newZ = func(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
	}

    void OnCollisionEnter (Collision collision) {
        EvaluateCollision(collision);
	}

	void OnCollisionStay (Collision collision) {
        EvaluateCollision(collision);
	}

    void EvaluateCollision (Collision collision) {
		for (int i = 0; i < collision.contactCount; i++) {
			Vector3 normal = collision.GetContact(i).normal;
		    if (normal.y>=minGroundDotProduct){
                lastOnGroundTime = coyoteTime;
                contactNormal = normal;
            }
            else{
                lastOnGroundTime = 0.0f;
                contactNormal = Vector3.up;
            }
		}
	}

    Vector3 ProjectOnContactPlane (Vector3 vector) {
		return vector - contactNormal * Vector3.Dot(vector, contactNormal);
	}
}