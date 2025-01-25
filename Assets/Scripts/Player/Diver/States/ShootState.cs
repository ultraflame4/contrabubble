using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class ShootState : CoroutineStateNetwork<DiverController>
    {
        float timeElasped = 0f;

        public ShootState(DiverController fsm) : base(fsm, fsm, fsm.Default, fsm.shootDuration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            timeElasped = 0f;
            // reset velocity
            character.rb.linearVelocity = Vector3.zero;
            // shoot projectile
            character.projectile.transform.localPosition = Vector3.zero;
            character.projectile.transform.up = character.pointer.transform.up;
            character.projectile.gameObject.SetActive(true);
            character.projectile.rb
            .AddForce(character.projectile.transform.up * Mathf.Lerp(character.minShootForce, character.maxShootForce, character.Charge.ShootForceScale), ForceMode.Impulse);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timeElasped += Time.deltaTime;
            // for last pullback window % of shoot duration, pull back projectile
            if (timeElasped < character.shootDuration * character.pullbackWindow) return;
            character.projectile.transform.position = Vector3.Lerp(character.projectile.transform.position, character.transform.position, Time.deltaTime * character.pullbackSpeed);
            // when projectile is back to player, hide it
            if (Vector3.Distance(character.projectile.transform.position, character.transform.position) > character.pullbackStopDistance) return;
            character.projectile.gameObject.SetActive(false);
        }

        public override void Exit()
        {
            base.Exit();
            character.projectile.gameObject.SetActive(false);
        }
    }
}

