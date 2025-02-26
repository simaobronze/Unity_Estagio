using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _jumpSpeed;

    [SerializeField]
    private LayerMask _groundLayer;

    private NinjaInputController _inputController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;
    private bool _isGrounded;

    private void Awake()
    {
        _inputController = GetComponent<NinjaInputController>();
        _rigidbody = GetComponent<Rigidbody>();

        _inputController.OnJumpButtonPressed += JumpButtonPressed;
    }

    private void FixedUpdate()
    {
        CheckGroundedStatus();

        Vector3 velocity = new Vector3(
            _inputController.MoveInputVector.x,
            0,
            _inputController.MoveInputVector.y
            ) 
            *_speed;

        velocity.y = _rigidbody.linearVelocity.y;

        if (_jumpTriggered && _isGrounded)
        {
            velocity.y = _jumpSpeed;
            _jumpTriggered = false;
        }

        _rigidbody.linearVelocity = velocity;
    }

    private void JumpButtonPressed()
    {
        _jumpTriggered = true;
    }

    private void CheckGroundedStatus()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, _groundLayer);

    }
}
