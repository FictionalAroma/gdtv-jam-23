using Enemy.States;
using UnityEngine;

namespace Enemy.Boss
{
	public class BossDeadState : EnemyBaseState
	{
		float deathTime = 3f;
		float deathTimer; 
		public BossDeadState(GameObject obj, EnemyState state = EnemyState.Dead) : base(obj, state)
		{
			deathTimer = deathTime;
		}

		public override EnemyState Tick()
		{
			deathTimer -= Time.deltaTime;
			if (deathTimer<= 0)
            {
				GameObject.Destroy(_gameObject);
            }
			return State;
		}

		public override void Activate()
		{
			Context.animator.SetTrigger("isDead");
			
			Context.Attacker.StopMoving();
		}

		public override void Deactivate()
		{
			Context.Attacker.Target = null;
		}
	}
}