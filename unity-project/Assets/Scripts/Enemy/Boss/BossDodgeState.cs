using UnityEngine;

namespace Enemy.Boss
{
    public class BossDodgeState : BossBaseState
    {
		public BossDodgeState(GameObject obj, BossState state = BossState.Idle) : base(obj, state)
		{

		}

        public override BossState Tick()
		{
			if (Context.Attacker.GetDistanceRemaining<2)
            {
				return BossState.Attack;
            }
			
			return BossState.Dodge;
		}

		public override void Activate()
		{
			Context.animator.SetTrigger("isDodging");
			Context.Attacker.JumpToPosition(Context.EnemyManager.startPosition.position);
			Context.EnemyManager.ResetPosition();
			for (int i = 0; i<Context.EnemyManager._colliderCache.Length;i++)
            {
				Context.EnemyManager._colliderCache[i].enabled = false;
            }
		}

		public override void Deactivate()
		{
			for (int i = 0; i < Context.EnemyManager._colliderCache.Length; i++)
			{
				Context.EnemyManager._colliderCache[i].enabled = true;
			}
			
		}
	}
}