using UnityEngine;

namespace Enemy.Boss
{
    public class BossIdleState : BossBaseState
    {
		public BossIdleState(GameObject obj, BossState state = BossState.Idle) : base(obj, state)
		{

		}

        public override BossState Tick()
		{
			return BossState.Idle;
		}

		public override void Activate()
		{
		}

		public override void Deactivate()
		{

		}
	}
}