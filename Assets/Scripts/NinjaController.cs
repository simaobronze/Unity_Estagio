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
    private float Health, MaxHealth = 100f;

    [SerializeField]
    private HealthBar _healthBar;

    private void Awake()
    {
        _inputController = GetComponent<NinjaInputController>();
        _rigidbody = GetComponent<Rigidbody>();

        _inputController.OnJumpButtonPressed += JumpButtonPressed;

        _healthBar.SetMaxHealth(MaxHealth);
        _healthBar.SetHealth(MaxHealth);

    }

    private void FixedUpdate()
    {
        CheckGroundedStatus();

        Vector3 moveDirection = new Vector3(
            _inputController.MoveInputVector.x,
            0,
            _inputController.MoveInputVector.y
        );

        Vector3 velocity = moveDirection * _speed;
        velocity.y = _rigidbody.linearVelocity.y;

        if (_jumpTriggered && _isGrounded)
        {
            velocity.y = _jumpSpeed;
            _jumpTriggered = false;
        }

        _rigidbody.linearVelocity = velocity;

        // Rotaciona o personagem na dire��o do movimento
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        if (Input.GetKeyDown("p"))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown("l"))
        {
            TakeDamage(-10f);
        }
    }

    private void JumpButtonPressed()
    {
        _jumpTriggered = true;
    }

    private void CheckGroundedStatus()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, _groundLayer);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        _healthBar.SetHealth(Health);
    }
}