using UnityEngine;

namespace Enemy.States
{
    public class SpawnerIdleState : EnemyBaseState
    {
		public SpawnerIdleState(GameObject obj, EnemyState state = EnemyState.Idle) : base(obj, state)
		{

		}

        public override EnemyState Tick()
		{

			return  EnemyState.Idle;
		}

		public override void Activate()
		{
			if (Context != null && Context.Attacker != null) Context.Attacker.Target = null;
		}

		public override void Deactivate() {}

		public override EnemyState DamageTaken(float amount) => EnemyState.Alert;
    }
}