using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class ChargeState : StateNetwork<DiverController>
    {
        public float ShootForceScale => Mathf.Clamp01(character.chargeDuration / character.maxChargeDuration);
        Transform parent => character.chargeSlider.transform.parent;

        public ChargeState(DiverController fsm) : base (fsm, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.chargeDuration = 0f;
            parent.gameObject.SetActive(true);
            character.anim?.Play("Charge");
        }

        public override void LogicUpdate() 
        {
            base.LogicUpdate();
            // force point up
            character.transform.up = Vector2.up;
            // update pointer
            character.pointer.UpdatePointer(character.aimVector);
            // check if need to flip sprite
            character.sprite.flipX = character.pointer.transform.localPosition.x >= 0f;
            // flip slider according to sprite
            parent.localScale = new Vector3(character.sprite.flipX ? -1f : 1f, 1f, 1f);
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
            parent.gameObject.SetActive(false);
        }
    }
}

