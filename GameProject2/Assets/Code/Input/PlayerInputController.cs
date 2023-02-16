using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.Animations;

public class PlayerInputController : NetworkBehaviour
{

    // Serialized fields for the Rigidbody component of the player and the move speed and jump force values
    [SerializeField] private Rigidbody _characterRB;
    [SerializeField] public float moveSpeed, jumpForce, dodgeForce, acceleration, deceleration, dodgeTime;

    [SerializeField] private Animator _animator;
    private float currentSpeed;

    // Private Vector2 field to store the player's movement input
    private Vector2 _move;

    // Vector2 to store the mouse position
    private Vector2 _mousePos;

    // Timestamp for the last time the player jumped, dodged and the minimum interval for jumping and dodging
    private float _jumpTimeStamp = 0;
    private float _dodgeTimeStamp = 0;
    public float minJumpInterval, minDodgeInterval;

    // Bool to check if the player is grounded
    private bool _isGrounded;
    private bool isDodging = false;
    private Vector3 dodgeDirection;

    void Awake()
    {
        _characterRB = GetComponent<Rigidbody>();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        var playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
    }


    // Call the MovePlayer method in the Update method
    void Update()
    {
        MovePlayer();
    }


    // Method to handle player movement input
    public void OnMove(InputAction.CallbackContext moveContext)
    {
        _move = moveContext.ReadValue<Vector2>();
    }

    // Method to update the player's position based on movement input
    public void MovePlayer()
    {
        if (!isLocalPlayer) return;
        if (isDodging) 
        {
            if (_dodgeTimeStamp + dodgeTime <= Time.time) 
            {
                isDodging = false;
                
            }
            else
            {
                transform.Translate(dodgeDirection * dodgeForce * Time.deltaTime, Space.World);
                return;
            }
        }

        var right = Camera.main.transform.right;
        var forward = Vector3.Cross(right, Vector3.up);

        // Calculate the movement vector
        var movementRight = right * _move.x;
        var movementForward = forward * _move.y;

        var movement = movementRight + movementForward;

        // Convert the movement to a direction vector relative to the character look direction
        var localMovement = transform.InverseTransformDirection(movement).normalized;

        if (_animator != null)
        {
            _animator.SetFloat("MoveSpeed", localMovement.x, 0.15f, Time.deltaTime);
            _animator.SetFloat("MoveSpeed", localMovement.z, 0.15f, Time.deltaTime);
        }

        transform.Translate(movement * currentSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        }

    }

    // This method is called whenever a "look" action is performed
    public void OnLook(InputAction.CallbackContext lookContext)
    {
        // Get the mouse position in screen coordinates
        Vector2 mousePosition = lookContext.ReadValue<Vector2>(); ;

        // Convert the mouse position to a ray that projects into the scene
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        // Define a plane that intersects with the scene at the object's position
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayDistance;

        // Check if the mouse ray intersects with the ground plane
        if (groundPlane.Raycast(mouseRay, out rayDistance))
        {
            // Get the point at which the mouse ray intersects with the ground plane
            Vector3 lookPoint = mouseRay.GetPoint(rayDistance);

            // Make the object look at the look point
            transform.LookAt(lookPoint);
        }
    }

    // Method to handle player jump input
    public void OnJump(InputAction.CallbackContext jumpContext)
    {
        // Check if the jump cooldown has ended
        bool jumpCooldownOver = (Time.time - _jumpTimeStamp) >= minJumpInterval;

        // If the jump cooldown has ended and the player is grounded, jump
        if (jumpCooldownOver && _isGrounded)
        {
            _jumpTimeStamp = Time.time;
            _characterRB.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    public void OnDodge(InputAction.CallbackContext dodgeContext)
    {
        // Check if the dodge cooldown has ended
        bool dodgeCooldownOver = (Time.time - _dodgeTimeStamp) >= minDodgeInterval;

        // If the dodge cooldown has ended and the player is grounded, dodge
        if (dodgeCooldownOver)
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Dash");
            }
            isDodging = true;
            _dodgeTimeStamp = Time.time;
            dodgeDirection = transform.forward;
        }
    }

    // Method to check if the player is touching the ground
    private void OnCollisionEnter(Collision col)
    {
        // If the player is touching a collider with the "Ground" tag, set _isGrounded to true
        if (col.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
    }
}
