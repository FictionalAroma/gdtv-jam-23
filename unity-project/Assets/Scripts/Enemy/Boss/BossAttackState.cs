using Enemy.States;
using UnityEngine;

namespace Enemy.Boss
{
	public class BossAttackState : BossBaseState
	{
		private static readonly int IsRunning = Animator.StringToHash("isRunning");

		private float activateOthersTimer = 1.0f;

		public BossAttackState(GameObject obj, BossState state = BossState.Attack) : base(obj, state)
		{
		}

		public override BossState Tick()
		{

			return BossState.Attack;
		}

		public override void Activate()
		{
			Context.Attacker.Target = Context.PlayerCache.transform;
			Context.Attacker.moveTo = false;
			int attackVariation = Random.Range(0, 3);
			Context.animator.SetBool("isMoving", false);
			Context.animator.SetInteger("attackVariation",attackVariation);
			if (attackVariation == 0)
            {
				Context.Attacker.TargetStoppingRange = 100;
            }
			else if (attackVariation == 1)
            {
				Context.Attacker.moveTo = true;
				Context.Attacker.TargetStoppingRange = 5;
            }
			else
            {
				Context.Attacker.JumpToPosition(Context.Attacker.Target.transform.position);
				Context.Attacker.TargetStoppingRange = 0;
            }
			Context.animator.SetTrigger("isAttacking");
			
			
		}

		public override void Deactivate()
		{
			Context.animator.ResetTrigger("isAttacking");
			Context.Attacker.moveTo = true;
			Context.animator.SetBool("isRunning", false);
			Context.Attacker.Target = null;
		}
	}
}