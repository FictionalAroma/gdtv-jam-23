using UnityEngine;

namespace Enemy.States
{
    public class EnemyIdleState : EnemyBaseState
    {
		public EnemyIdleState(GameObject obj, EnemyState state = EnemyState.Idle) : base(obj, state)
		{

		}

        public override EnemyState Tick()
		{
			var canSeePlayer = CanSeePlayer();

			return canSeePlayer ? EnemyState.Alert : EnemyState.Idle;
		}

		public override void Activate()
		{
			
			Context.Attacker.Target = null;
		}

		public override void Deactivate() {}

		public override EnemyState DamageTaken(float amount) => EnemyState.Alert;
    }
}