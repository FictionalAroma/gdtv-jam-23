using Enemy.States;
using UnityEngine;

namespace Enemy.Boss
{
	public class BossAlertState : BossBaseState
	{
		private static readonly int IsRunning = Animator.StringToHash("isRunning");

		private float activateOthersTimer = 1.0f;

		public BossAlertState(GameObject obj, BossState state = BossState.Intro) : base(obj, state)
		{
		}

		public override BossState Tick()
		{
			return State;
		}

		public override void Activate()
		{
			//Context.animator.SetBool(IsRunning,true);
			//Context.Attacker.Target = Context.PlayerCache.transform;
			//Context.Attacker.moveTo = true;
			//Context.Attacker.StartShooting = true;
		}

		public override void Deactivate()
		{
			//Context.animator.SetBool("isRunning", false);
			//Context.Attacker.Target = null;
		}
	}
}