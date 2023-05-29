using UnityEngine;

namespace Enemy.States
{
    public class SpawnerAlertState : EnemyBaseState
    {
		private float _alertReset = 10;

		public SpawnerAlertState(GameObject obj, EnemyState state = EnemyState.Alert) : base(obj, state)
		{

		}

        public override EnemyState Tick()
		{
			_alertReset -= Time.deltaTime;
			if (_alertReset < 0)
			{
				return EnemyState.Idle;
			}

			return EnemyState.Alert;
		}

		public override void Activate()
		{
			_alertReset = 10;
		}

		public override void Deactivate()
		{

		}

		public override EnemyState DamageTaken(float amount) => EnemyState.Attack;
    }
}