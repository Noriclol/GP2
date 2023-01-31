using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody _characterRB;
    [SerializeField] public float moveSpeed, jumpForce;

    private Vector2 _move;

    private float _jumpTimeStamp = 0;
    public float minJumpInterval;

    private bool _isGrounded;



    void Awake()
    {
        _characterRB = GetComponent<Rigidbody>();
    }


    void Update()
    {
        MovePlayer();
    }


    // movement
    public void OnMove(InputAction.CallbackContext moveContext)
    {
        _move = moveContext.ReadValue<Vector2>();
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(_move.x, 0, _move.y);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
    }


    // jump function
    public void OnJump(InputAction.CallbackContext jumpContext)
    {
        bool jumpCooldownOver = (Time.time - _jumpTimeStamp) >= minJumpInterval;

        if(jumpCooldownOver && _isGrounded)
        {
            _jumpTimeStamp = Time.time;
            _characterRB.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    // Groundcheck
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
    }
}
