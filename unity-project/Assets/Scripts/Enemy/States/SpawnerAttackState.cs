using UnityEngine;

namespace Enemy.States
{
    public class SpawnerAttackState : EnemyBaseState
    {

		public SpawnerAttackState(GameObject obj, EnemyState state = EnemyState.Attack) : base(obj, state)
		{

		}

        public override EnemyState Tick()
		{
			return EnemyState.Attack;
		}

		public override void Activate()
		{
			var spawner = _gameObject.GetComponent<EnemySpawner>();
			spawner.StartSpawn();
		}

		public override void Deactivate()
		{

		}

		public override EnemyState DamageTaken(float amount) => EnemyState.Attack;
    }
}