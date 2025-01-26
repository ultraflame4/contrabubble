using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DefaultState : StateNetwork<DiverController>
    {
        float dot;
        Vector2 targetRotation;

        public DefaultState(DiverController fsm) : base (fsm, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.anim?.Play("Idle");
        }

        public override void LogicUpdate() 
        {
            base.LogicUpdate();

            // update pointer
            character.pointer.UpdatePointer(character.aimVector);

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
            if (character.moveInput.magnitude == 0f)
            {
                targetRotation = Vector2.up;
            }
            else
            {
                targetRotation = character.moveInput;
                // if movement is not zero, move player
                character.rb.AddForce(character.transform.up * character.movementSpeed * Time.deltaTime);
            }

            dot = Vector2.Dot(character.transform.up, targetRotation);

            // once reached target rotation, snap to rotation
            if (dot >= character.rotationMatchThreshold)
            {
                character.transform.up = targetRotation;
                return;
            }

            // turn to side if movement vector is complete opposite direction
            if (dot <= -character.rotationMatchThreshold)
            {
                dot = Vector3.Dot(character.transform.right, character.pointer.transform.up);
                targetRotation = character.transform.right * (dot < 0f ? -1f : 1f);
            }

            character.transform.up = Vector2.Lerp(character.transform.up, targetRotation, character.rotationSpeed * Time.deltaTime);
        }

        void HandleSprite()
        {
            if (character.moveInput == Vector3.zero) return;

            if (character.moveInput.y != 0f && character.moveInput.x == 0f)
                character.sprite.flipX = character.pointer.transform.localPosition.x >= 0f;
            else
               character.sprite.flipX = character.moveInput.x >= 0f;
        }
    }
}
