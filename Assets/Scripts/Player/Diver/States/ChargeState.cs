using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class ChargeState : StateNetwork<DiverController>
    {
        float chargeDuration = 0f;

        Transform canvas => character.chargeSlider.transform.parent;

        public float ShootForceScale => Mathf.Clamp(chargeDuration / character.chargeDuration, 0f, 1f);

        public ChargeState(DiverController fsm) : base (fsm, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            chargeDuration = 0f;
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
            chargeDuration += Time.deltaTime;
            if (character.shootInput && chargeDuration < character.chargeDuration) return;
            fsm.SwitchState(character.Shoot);
        }

        public override void Exit() 
        {
            base.Exit();
            canvas.gameObject.SetActive(false);
        }
    }
}

