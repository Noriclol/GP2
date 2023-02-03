using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // Serialized fields for the Rigidbody component of the player and the move speed and jump force values
    [SerializeField] private Rigidbody _characterRB;
    [SerializeField] public float moveSpeed, jumpForce, dodgeForce;

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

    // Projectile 
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed;

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

        //// If there is movement input, rotate the player towards the movement direction
        //if (movement != Vector3.zero)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        //}
    }

    // This method is called whenever a "look" action is performed
    public void OnLook(InputAction.CallbackContext lookContext)
    {
        // Get the mouse position in screen coordinates
        Vector2 mousePosition = Input.mousePosition;

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


    // Method to handle player action input
    public void OnShoot(InputAction.CallbackContext shootContext)
    {
        if (shootContext.performed)
        {
            Vector3 shootDirection = transform.forward;
            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            newProjectile.GetComponent<Rigidbody>().velocity = shootDirection * projectileSpeed;

            Destroy(newProjectile, 5f);
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
            _dodgeTimeStamp = Time.time;
            Vector3 dodgeDirection = transform.forward;
            _characterRB.AddForce(dodgeDirection * dodgeForce, ForceMode.Impulse);
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
