using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RigidBodyContoller : MonoBehaviour
{
	private float COMPENSATOR = 10f;

	[SerializeField] private float _speed;
	[SerializeField] private float _angularSpeed;
	[SerializeField] private float _jumpPower;

	private Rigidbody _rb;
	private CapsuleCollider _cc;
	private Animator _animator;

	private bool _isGrounded;

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_cc = GetComponent<CapsuleCollider>();
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		IsGrounded();
		Jump();
	}

	private void FixedUpdate()
	{
		Movement();
	}

	private void Movement()
	{
		if (_isGrounded)
		{
			float mX = Input.GetAxis("Horizontal");
			float mY = Input.GetAxis("Vertical");

			if (mY < 0)
			{
				mX = -mX;
				Animate(0.2f);
			}
			else
			{
				Animate(mY);
			}

			Vector3 directionMovement = transform.forward * mY * _speed * Time.fixedDeltaTime * COMPENSATOR;
			directionMovement.y = _rb.velocity.y;

			Vector3 directionRotate = _rb.angularVelocity;
			directionRotate.y = mX * _angularSpeed * Time.fixedDeltaTime * COMPENSATOR;

			_rb.velocity = directionMovement;
			_rb.angularVelocity = directionRotate;
		}
		else
		{
			_rb.angularVelocity = Vector3.zero;
			Animate(0.1f);
		}
	}

	private void Animate(float speed)
	{
		_animator.SetFloat("Speed", speed);
	}

	private void Jump()
	{
		if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			_rb.AddForce(Vector3.up * _jumpPower * COMPENSATOR);
		}
	}

	private void IsGrounded()
	{
		Vector3 centerDownSphere = transform.position + Vector3.up * _cc.radius;
		Vector3 centerUpSphere = transform.position + Vector3.up * (_cc.height - _cc.radius);

		RaycastHit raycastHit;
		Physics.CapsuleCast(centerDownSphere, centerUpSphere, _cc.radius, Vector3.down, out raycastHit, 0.1f);

		if (raycastHit.transform && raycastHit.transform != transform)
		{
			_isGrounded = true;
		}
		else
		{
			_isGrounded = false;
		}
	}
}