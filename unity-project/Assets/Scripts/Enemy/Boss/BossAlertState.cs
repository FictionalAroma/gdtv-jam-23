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
			Context.animator.SetTrigger("isAlert");
			
		}

		public override void Deactivate()
		{
			Context.animator.SetBool("isMoving", true);
			
		}
	}
}