using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DefaultState : State<DiverController>
    {
        float dot;
        Vector2 targetRotation;

        public DefaultState (StateMachine<DiverController> fsm, DiverController character) : base (fsm, character)
        {
        }

        public override void LogicUpdate() 
        {
            base.LogicUpdate();

            // update pointer
            character.pointer.UpdatePointer();

            // check transitions
            if (character.shootInput)
            {
                fsm.SwitchState(character.Charge);
                return;
            }

            HandleMovement();
            HandleSprite();
        }

        void HandleMovement()
        {
            if (character.moveInput.magnitude == 0f) return;

            character.rb.AddForce(character.transform.up * character.movementSpeed * Time.deltaTime);

            dot = Vector2.Dot(character.transform.up, character.moveInput);

            if (dot >= character.rotationMatchThreshold)
            {
                character.transform.up = character.moveInput;
                return;
            }

            if (dot > -character.rotationMatchThreshold)
            {
                targetRotation = character.moveInput;
            }
            else
            {
                dot = Vector3.Dot(character.transform.right, character.pointer.transform.up);
                targetRotation = character.transform.right * (dot < 0f ? -1f : 1f);
            }

            character.transform.up = Vector2.Lerp(character.transform.up, targetRotation, character.rotationSpeed * Time.deltaTime);
        }

        void HandleSprite()
        {
            character.sprite.flipX = character.rb.linearVelocity.x < 0f;
            // character.sprite.flipX = character.rb.linearVelocity.y >= 0f;
        }
    }
}
