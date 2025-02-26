using UnityEngine;

public class NinjaController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private NinjaInputController _inputController;

    private void Awake()
    {
        _inputController = GetComponent<NinjaInputController>();
    }

    private void Update()
    {
        Vector3 positionChange = new Vector3(
            _inputController.MoveInputVector.x,
            0,
            _inputController.MoveInputVector.y
            ) 
            * Time.deltaTime
            *_speed;

        transform.position += positionChange;
    }
}
