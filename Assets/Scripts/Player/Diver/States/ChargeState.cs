using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class ChargeState : StateNetwork<DiverController>
    {
        public float ShootForceScale => Mathf.Clamp01(character.chargeDuration / character.maxChargeDuration);
        Transform canvas => character.chargeSlider.transform.parent;

        public ChargeState(DiverController fsm) : base (fsm, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.chargeDuration = 0f;
            canvas.gameObject.SetActive(true);
        }

        public override void LogicUpdate() 
        {
            base.LogicUpdate();
            // update pointer
            character.pointer.UpdatePointer(character.aimVector);
            // check if need to flip sprite
            character.sprite.flipX = character.pointer.transform.localPosition.x < 0f;
            // offset canvas rotation for charge bar
            canvas.localRotation = Quaternion.Euler(-character.transform.eulerAngles);
            // update charge value
            character.chargeSlider.value = ShootForceScale;
            // increment charge duration
            if (character.chargeDuration < character.maxChargeDuration) 
                character.chargeDuration += Time.deltaTime;
            // only release when shoot input is released
            if (character.shootInput) return;
            fsm.SwitchState(character.Shoot);
        }

        public override void Exit() 
        {
            base.Exit();
            canvas.gameObject.SetActive(false);
        }
    }
}

