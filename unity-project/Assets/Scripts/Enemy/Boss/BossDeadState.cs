using UnityEngine;

namespace Enemy.Boss
{
	public class BossDeadState : BossBaseState
	{
		float deathTime = 3f;
		float deathTimer; 
		public BossDeadState(GameObject obj, BossState state = BossState.Dead) : base(obj, state)
		{
			deathTimer = deathTime;
		}

		public override BossState Tick()
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
			Context.animator.SetBool("isDead",true);
			Context.animator.SetTrigger("Death");
			
			Context.Attacker.StopMoving();
		}

		public override void Deactivate()
		{
			Context.Attacker.Target = null;
		}
	}
}