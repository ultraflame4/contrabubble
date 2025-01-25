using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class DefaultState : State<DiverController>
    {
        float rotationDot;
        Vector2 targetRotation;

        public DefaultState (StateMachine<DiverController> fsm, DiverController character) : base (fsm, character)
        {
        }

        public override void LogicUpdate() 
        {
            base.LogicUpdate();

            if (character.moveInput.magnitude == 0f) return;

            character.rb.AddForce(character.transform.up * character.movementSpeed * Time.deltaTime);

            rotationDot = Vector2.Dot(character.transform.up, character.moveInput);

            if (rotationDot >= character.rotationMatchThreshold)
            {
                character.transform.up = character.moveInput;
                return;
            }


            if (rotationDot <= -character.rotationMatchThreshold)
                targetRotation = character.transform.right;
            else
                targetRotation = character.moveInput;

            character.transform.up = Vector2.Lerp(character.transform.up, targetRotation, character.rotationSpeed * Time.deltaTime);
        }
    }
}
