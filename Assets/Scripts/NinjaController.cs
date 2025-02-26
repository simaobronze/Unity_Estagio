using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _jumpSpeed;

    private NinjaInputController _inputController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;

    private void Awake()
    {
        _inputController = GetComponent<NinjaInputController>();
        _rigidbody = GetComponent<Rigidbody>();

        _inputController.OnJumpButtonPressed += JumpButtonPressed;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(
            _inputController.MoveInputVector.x,
            0,
            _inputController.MoveInputVector.y
            ) 
            *_speed;

        velocity.y = _rigidbody.linearVelocity.y;

        if (_jumpTriggered)
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
}
