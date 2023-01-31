using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // Serialized fields for the Rigidbody component of the player and the move speed and jump force values
    [SerializeField] private Rigidbody _characterRB;
    [SerializeField] public float moveSpeed, jumpForce;

    // Private Vector2 field to store the player's movement input
    private Vector2 _move;

    // Timestamp for the last time the player jumped and the minimum interval for jumping
    private float _jumpTimeStamp = 0;
    public float minJumpInterval;

    // Bool to check if the player is grounded
    private bool _isGrounded;



    void Awake()
    {
        _characterRB = GetComponent<Rigidbody>();
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
        // Calculate the movement vector
        Vector3 movement = new Vector3(_move.x, 0, _move.y);

        // Translate the player's position based on the movement vector
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // If there is movement input, rotate the player towards the movement direction
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
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
