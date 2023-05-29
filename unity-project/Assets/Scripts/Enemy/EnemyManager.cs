using CommonComponents;
using Enemy.States;
using UI;
using UnityEngine;

namespace Enemy
{
	[RequireComponent(typeof(SliderDisplay))]
	[RequireComponent(typeof(EnemyStateMachine))]
	public class EnemyManager : Damagable
	{
		protected EnemyStateMachine _stateMachine;
		public Animator enemyAnimator;
		protected EnemyMover _mover;

		protected SliderDisplay _hpBar;
        public float lookDistance = 30f;
		public float lookAngle = 60f;
		private bool _attackImmediate = false;

		protected override void Awake()
		{
			_stateMachine = GetComponent<EnemyStateMachine>();


			base.Awake();

			_hpBar = GetComponent<SliderDisplay>();
			HPChangedEvent += _hpBar.SetValues;
			HPEmpty += OnDeath;
			
			_mover = GetComponent<EnemyMover>();

		}

		private void Start()
		{
			DamageTaken += _stateMachine.DamageTaken;
			_stateMachine.AddState(new EnemyIdleState(gameObject));
			_stateMachine.AddState(new EnemyAttackState(gameObject));
			_stateMachine.AddState(new EnemyAlertState(gameObject));
			_stateMachine.AddState(new EnemyDeadState(gameObject));

			if (_attackImmediate)
			{
				_stateMachine.TrySwapState(EnemyState.Attack);
			}
		}

		public void ImmediateAttack(Transform newPos)
		{
			_mover.Target = newPos;
			_attackImmediate = true;
		}

		protected void OnDeath(Damagable damagable)
		{
			_stateMachine.SwapState(EnemyState.Dead);
			
		}
		

	}

}