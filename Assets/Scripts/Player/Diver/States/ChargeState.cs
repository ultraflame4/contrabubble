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
            // offset canvas rotation for charge bar
            canvas.localRotation = Quaternion.Euler(-character.transform.eulerAngles);
            // update charge value
            character.chargeSlider.value = ShootForceScale;
            // check charge duration
            character.chargeDuration += Time.deltaTime;
            if (character.shootInput && character.chargeDuration < character.maxChargeDuration) return;
            fsm.SwitchState(character.Shoot);
        }

        public override void Exit() 
        {
            base.Exit();
            canvas.gameObject.SetActive(false);
        }
    }
}

