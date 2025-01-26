using UnityEngine;
using Utils.Patterns.FSM;

namespace Player.Diver
{
    public class ShootState : CoroutineStateNetwork<DiverController>
    {
        Bubble collectedBubble = null;

        public ShootState(DiverController fsm) : base(fsm, fsm, fsm.Default, fsm.shootDuration)
        {
        }

        public override void Enter()
        {
            base.Enter();
            character.timeInShoot = 0f;
            character.shootHit = false;
            collectedBubble = null;
            // set projectile flip
            character.projectile.isFlipped = character.sprite.flipX;
            // activate collider
            character.projectile.col.enabled = true;
            // shoot projectile
            character.projectile.transform.position = character.projectile.startPos;
            character.projectile.transform.up = character.pointer.transform.up;
            character.projectile.gameObject.SetActive(true);
            character.projectile.rb
                .AddForce(character.projectile.transform.up * Mathf.Lerp(character.minShootForce, character.maxShootForce, character.Charge.ShootForceScale), ForceMode.Impulse);
            // subscribe to events
            if (!character.IsOwner) return;
            character.projectile.OnCollectBubble += OnCollectBubble;
            character.projectile.OnHit += OnHit;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // increment time
            character.timeInShoot += Time.deltaTime;
            // for last pullback window % of shoot duration, pull back projectile
            if (!character.shootHit && character.timeInShoot < character.shootDuration * (1f - character.pullbackWindow)) return;
            character.projectile.col.enabled = false;
            character.projectile.rb.linearVelocity = Vector3.zero;
            character.projectile.transform.position = Vector3.Lerp(character.projectile.transform.position, character.projectile.startPos, Time.deltaTime * character.pullbackSpeed);
            if (collectedBubble != null) collectedBubble.transform.position = character.projectile.transform.position;
            // when projectile is back to player, hide it
            if (Vector3.Distance(character.projectile.transform.position, character.transform.position) > character.pullbackStopDistance) return;
            character.projectile.gameObject.SetActive(false);
            // return to original state if character.shootHit
            if (!character.shootHit) return;
            fsm.SwitchState(nextState);
        }

        public override void Exit()
        {
            base.Exit();
            character.projectile.gameObject.SetActive(false);
            // unsubscribe from events
            character.projectile.OnCollectBubble -= OnCollectBubble;
            character.projectile.OnHit -= OnHit;
            // collect bubble
            if (collectedBubble == null) return;
            collectedBubble.Collected(character.bubbleStorage);
        }

        void OnCollectBubble(Bubble bubble)
        {
            character.shootHit = true;
            // collect bubble
            collectedBubble = bubble;
            collectedBubble.DeactivateRPC();
            // unsubscribe from events on character.shootHit
            character.projectile.OnCollectBubble -= OnCollectBubble;
            character.projectile.OnHit -= OnHit;
        }

        void OnHit(BubbleStorage storage)
        {
            character.shootHit = true;
            // unsubscribe from events on character.shootHit
            character.projectile.OnCollectBubble -= OnCollectBubble;
            character.projectile.OnHit -= OnHit;
        }
    }
}

