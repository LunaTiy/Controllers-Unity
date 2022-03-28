using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    [SerializeField] private float _speedMovement;
    [SerializeField] private float _angularSpeed;
	[SerializeField] private float _jumpPower;
	[Space]
	[SerializeField] private float _gravity;

    private CharacterController _characterController;
	private Animator _animator;

	private float _moveVertical;

	private void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		Gravity();
		Movement();
	}

	private void Movement()
	{
		float mX = Input.GetAxis("Horizontal");
		float mY = Input.GetAxis("Vertical");

		if (mY < 0)
		{
			mX = -mX;
			AnimateMovement(0.1f);
		}
		else
		{
			AnimateMovement(mY);
		}

		Vector3 directionRotate = new Vector3(0f, mX * _angularSpeed * Time.deltaTime, 0f);
		Vector3 directionMovement = transform.forward * mY * _speedMovement * Time.deltaTime;
		directionMovement.y = _moveVertical;

		_characterController.Move(directionMovement);
		transform.Rotate(directionRotate);
	}

	private void Gravity()
	{
		if (!_characterController.isGrounded) _moveVertical -= _gravity * Time.deltaTime;
		else _moveVertical = -1f;

		if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
		{
			_moveVertical = _jumpPower;
		}
	}

	private void AnimateMovement(float speed)
	{
		_animator.SetFloat("Speed", speed);
	}
}
