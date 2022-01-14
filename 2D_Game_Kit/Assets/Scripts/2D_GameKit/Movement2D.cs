using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TwoDGameKit
{
    /// <summary>
    /// Physics based 2D movement that expects Gravity
    /// Base Functionality includes left and right movement as well as jumping
    /// and Falling
    /// When we move we will tell the Animation Controller to Update
    /// When we jump we will tell the Animation Controller to Update
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement2D : MonoBehaviour
    {
        
        //Animation Controller
        Animator animator;
        //Rigidbody2D
        new private Rigidbody2D rigidbody;
        //Collider2D
        new private Collider2D collider;
        
        private bool isOnGround = true;
        private bool isJumping = false;

        [SerializeField,
            Tooltip("Speed to move the character")]
        private float speed = 10.0f;
        [SerializeField,
            Tooltip("Force to add on jump")]
        private float jumpForce;

        //Direction to move on fixed update
        public Vector2 moveDirection;

        // Start is called before the first frame update
        private void Start()
        {
            // attach the references we know about
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();

            // look for a valid Collider
            collider = GetComponent<Collider2D>();
            //Error if we do not find a valid reference
            if (collider == null)
            {
                //Log the error, attempt to attach a circle collider
                Debug.LogError("This object must have a 2D collider", gameObject);
                try
                {
                    gameObject.AddComponent<CircleCollider2D>();
                }
                catch
                {
                    Debug.Log("Unable to attach CircleCollider2D");
                    Destroy(this);
                    return;
                }
                Debug.LogWarning("Circle Collider Attached");
            }
        }
        private void FixedUpdate()
        {
            //move the rigidbody
            rigidbody.AddForce(moveDirection * speed);

            //clamp the max speed;
            Vector2 velocity = rigidbody.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -speed, speed);

            //if we are not using gravity prevent going diagonal faster
            if(rigidbody.gravityScale == 0)
            {
                velocity = velocity.normalized * speed;
                rigidbody.velocity = velocity;
            }
            //is jumping and we are using gravity
            else if(isJumping)
            {
                rigidbody.velocity = velocity;
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            

            //reset values
            moveDirection = Vector2.zero;
            isJumping = false;
        }


        /// <summary>
        /// Handle Movement
        /// </summary>
        /// <param name="direction">Direction of travel, positive is right</param>
        internal void Move(float direction)
        {
            //All movement will go through 2D movement with a y value of zeror
            Move(Vector2.right * direction);
        }
        /// <summary>
        /// Can handle movement on a 2D plane
        /// Use this when gravity is not used on the moving character
        /// </summary>
        /// <param name="direction">Vector 2 for directions</param>
        internal void Move(Vector2 direction)
        {
            moveDirection = direction;
            if(direction.x == 0 && isOnGround)
            {
                Vector2 stopVelocity = rigidbody.velocity;
                stopVelocity.x = 0;
                rigidbody.velocity = stopVelocity;

            }
        }

        /// <summary>
        /// Add force up if we are grounded
        /// </summary>
        internal void Jump()
        {
            if (isOnGround)
            {
                isJumping = true;
                isOnGround = false;
            }
        }

        /// <summary>
        /// Manage Collisions when they start to touch
        /// </summary>
        /// <param name="collision">object we are colliding with</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Manage when we hit the ground
            if (collision.gameObject.CompareTag("Ground"))
            {
                isOnGround = true;
            }
            /* Other Collisions */


        }
       
    }
}
