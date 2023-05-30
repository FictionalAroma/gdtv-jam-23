using UnityEngine;

namespace Enemy.States
{
	public class EnemyAttackState : EnemyBaseState
	{
		private static readonly int IsRunning = Animator.StringToHash("isRunning");

		private float activateOthersTimer = 1.0f;

		public EnemyAttackState(GameObject obj, EnemyState state = EnemyState.Attack) : base(obj, state)
		{
		}

		public override EnemyState Tick()
		{
			activateOthersTimer -= Time.deltaTime;
			if (activateOthersTimer < 0)
			{
				var enemiesToActivate = GetEnemiesToActivate();
				foreach (var hit in enemiesToActivate)
				{
					if (hit.collider.TryGetComponent<EnemyStateMachine>(out var mach))
					{
						mach.TrySwapState(EnemyState.Attack);
					}
				}

				activateOthersTimer = 3;
			}

			return State;
		}

		public override void Activate()
		{
			
			Context.Attacker.Target = Context.PlayerCache.transform;
			Context.Attacker.moveTo = true;
			Context.Attacker.StartShooting = true;
		}

		public override void Deactivate()
		{
			Context.animator.SetBool("isRunning", false);
			Context.Attacker.Target = null;
		}
	}
}