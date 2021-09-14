using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwoDGameKit
{
    /// <summary>
    /// Player controller is the main interface for dealing with player inputs
    /// We can move jump and accept damage or buffs through the player controller 
    /// Used for accepting inputs from the player to place on the character
    /// </summary>
    [RequireComponent(typeof(Movement2D))]
    [RequireComponent(typeof(SoundManager))]
    public class PlayerController : MonoBehaviour
    {
        // Movement controller
        private Movement2D movement;
         
        //R uns once when the object is created
        private void Start()
        {
            // Attach the movement controller
            movement = GetComponent<Movement2D>();
        }

        //Runs each frame
        private void Update()
        {
            // Check for inputs
            CheckForPlayerInput();

            /* Rest of update code */

        }

        //check for any buttons pressed, lifted or held during this frame
        private void CheckForPlayerInput()
        {
            // Check for inputs and activate corresponding actions
            ///----------
            /// Movement
            ///----------
            // Horizontal Movement
            // If there is Horizontal input (raw is -1.0 or 0.0 or 1.0)

            float direction = Input.GetAxisRaw("Horizontal");
            if ( direction != 0.0f)
            {
                movement.Move(direction);
            }
            // Jumping
            // try to jump if we are jumping
            bool willJump = Input.GetAxisRaw("Jump")!=0;

            if (willJump)
            {
                movement.Jump();
            }

            ///----------
            /// Actions
            ///----------

            /* More Actions */

        }

    }
}
