using System.Collections.Generic;
using CommonComponents;
using Enemy.States;
using Environment;
using UI;
using UnityEngine;

namespace Enemy.Boss
{
	[RequireComponent(typeof(SliderDisplay))]
	[RequireComponent(typeof(EnemyStateMachine))]
	[RequireComponent(typeof(BossMover))]
	public class BossManager : Damagable
	{

		public class BossManager : Damagable
		{
			protected EnemyStateMachine _stateMachine;
			public Animator enemyAnimator;
			protected BossMover _mover;

			private Collider[] _colliderCache;

			protected SliderDisplay _hpBar;
			public float lookDistance = 30f;
			public float lookAngle = 60f;
			private bool _attackImmediate = false;
			[SerializeField] private float TargetDistanceMelee;

			[SerializeField] private List<ObjectiveDoor> rechargePoints;

			protected override void Awake()
			{
				_stateMachine = GetComponent<EnemyStateMachine>();

				_colliderCache = GetComponents<Collider>();
				base.Awake();

				_hpBar = GetComponent<SliderDisplay>();
				HPChangedEvent += _hpBar.SetValues;
				HPEmpty += OnDeath;

				_mover = GetComponent<BossMover>();

			}

			private void Start()
			{
				DamageTaken += _stateMachine.DamageTaken;
				_stateMachine.AddState(new BossId(gameObject));
				_stateMachine.AddState(new EnemyAttackState(gameObject));
				_stateMachine.AddState(new BossDeadState(gameObject));

				if (_attackImmediate)
				{
					_stateMachine.TrySwapState(EnemyState.Attack);
				}
			}


			private void OnDeath(Damagable damagable)
			{
				ObjectiveDoor objective = null;
				foreach (var c in _colliderCache)
				{
					c.enabled = false;
				}
				foreach (var point in rechargePoints)
				{
					if (point.Locked)
					{
						objective = point;
					}
				}

				if (objective != null)
				{
					JumpToRecharge(objective);
				}
				else
				{
					_stateMachine.SwapState(EnemyState.Dead);
				}
			}

			private void JumpToRecharge(ObjectiveDoor objectiveDoor)
			{
				_mover.JumpToPosition(objectiveDoor.transform.position);

			}

			public void SwitchToMelee()
			{
				_mover.TargetStoppingRange = this.TargetDistanceMelee;
			}

			public void SwitchToRange()
			{

			}
		}

	}
}