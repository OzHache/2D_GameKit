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
        ///----------
        /// Public Scalars
        ///----------
        // Movement based values we want to change without bringing up our code
        [Header("Movement")]
        [SerializeField, Range(0.0f, 10.0f),
            Tooltip("top velocity in units/second")]
        private float topSpeed = 1.0f;

        [SerializeField, Range(0.0f, 5.0f),
            Tooltip("Time to full speed")]
        private float accelerationTime = 1.25f;

        [SerializeField,
            Tooltip("Acceleration curve")]
        private AnimationCurve accelerationCurve;

        [SerializeField, Range(0.0f, 5.0f),
            Tooltip("Time from full speed to stop")]
        private float decelerationTime = 0.25f;

        [SerializeField,
            Tooltip("Deceleration curve")]
        private AnimationCurve decelerationCurve;

        [Header("Jump")]
        [SerializeField, Range(0.0f, 10.0f),
            Tooltip("Force applied to a jump")]
        private float jumpForce = 1.0f;
        


        //Animation Controller
        Animator animator;
        //Rigidbody2D
        new private Rigidbody2D rigidbody;
        //Collider2D
        new private Collider2D collider;
        
        private bool isOnGround = true;
        private bool isMoving = false;

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
                //Log the error and destory the Script
                Debug.LogError("This object must have a 2D collider", gameObject);
                Destroy(this);
            }
        }

        /// <summary>
        /// Handle Movement
        /// </summary>
        /// <param name="direction">Direction of travel, positive is right</param>
        internal void Move(float direction)
        {
            //set is moving to true for this frame
            isMoving = true;

            // Get the adjusted speed
            float velocityX = SpeedAdjustment(accelerationTime, accelerationCurve, false);
            
            // Apply the force in the direction of travel
            rigidbody.velocity += new Vector2(velocityX * direction, 0);
        }

        /// <summary>
        /// Add force up if we are grounded
        /// </summary>
        internal void Jump()
        { 
            // Check for ground
            if (isOnGround)
            {
                // Impulse is an instant change that is affected by mass
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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

        /// <summary>
        /// Update that occurs after update but before the frame is rendered
        /// </summary>
        private void LateUpdate()
        {
            //if we are not updating movement this frame then slow us down
            if (!isMoving)
            {
                SlowDown();
            }
            isMoving = false;
        }

        /// <summary>
        /// Method for slowing the character down if we are moving and the
        /// movement buttons are not being pressed
        /// </summary>
        private void SlowDown()
        {
            float direction = 0.0f;
            if(Mathf.Abs(rigidbody.velocity.x) < decelerationTime)      // velocity is smaller than the lowest step
            {
                return;                                                 //let gravity and drag slow us
            }
            else if(rigidbody.velocity.x > 0)                           //going right
            {
                direction = 1.0f;
            }
            else                                                        //going left
            {
                direction = -1.0f;
            }
            var velocityX = SpeedAdjustment(decelerationTime, decelerationCurve, true);
            // Apply the force in the direction of travel
            rigidbody.velocity += new Vector2(velocityX * direction, 0);
        }



       /// <summary>
       /// Calculate the speed change from the current speed using the next step
       /// in the given speed curve
       /// </summary>
       /// <param name="speedStep">Step size per unit (second)</param>
       /// <param name="speedCurve">Acceleration or deceleration curve</param>
       /// <param name="direction"> expecting either 1.0 or -1.0f</param>
       /// <returns>the value of speed change based on the speed curve position</returns>
        private float SpeedAdjustment(float speedStep, AnimationCurve speedCurve, bool isSlowingDown)
        {
            /* 1) find the current position on the speed curve
             * 2) adjust to the next position by the step amount * time.deltaTime
             * 3) return the adjusted speed
             */
            // Return value for the velocity change
            float velocityChange = 0.0f;
            //size of the next step
            float stepSize = (1.0f / speedStep) * Time.deltaTime;
            // 1.a find the current velocity (x only and we want just the absolute value) 
            float currentVelocity = Mathf.Abs(rigidbody.velocity.x);
            //if we have stopped and we are slowing down, then we are done
            if(currentVelocity == 0 && isSlowingDown)
            {
                return 0.0f;
            }

            float speedCurvePosition = 0.0f;
            // If we are currently moving forward but have no velocity then make the current position the next step
            if (currentVelocity == 0)
            {
                speedCurvePosition = stepSize;
            }
            else
            {
                // Raw position (Step 1)
                speedCurvePosition = currentVelocity / topSpeed;
                // Adjusted position (Step 2)
                if (isSlowingDown)
                {
                    speedCurvePosition -= stepSize;
                }
                else
                {
                    speedCurvePosition += stepSize;
                }
            }
            // Step 3
            float newSpeed = topSpeed * speedCurve.Evaluate(speedCurvePosition);

            velocityChange = newSpeed - currentVelocity;
            
            return velocityChange ;
        }
    }
}
