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
		protected EnemyStateMachine _stateMachine;
		public Animator BossAnimator;
		protected BossMover _mover;

		public Collider[] _colliderCache;

		protected SliderDisplay _hpBar;
		[SerializeField] private float TargetDistanceMelee;
		[SerializeField] private float TargetDistanceRanged;

		[SerializeField] private List<ObjectiveDoor> rechargePoints;
		[SerializeField] public Vector3 startPosition;
		public bool startEncounter;
		public bool isAlreadyStarted;
		protected override void Awake()
		{
			_stateMachine = GetComponent<EnemyStateMachine>();
			isAlreadyStarted = false;
			startEncounter = false;
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
			_stateMachine.AddState(new BossIdleState(gameObject));
			_stateMachine.AddState(new BossAlertState(gameObject));
			_stateMachine.AddState(new BossDeadState(gameObject));

			foreach (var rechargePoint in rechargePoints)
			{
				rechargePoint.OnOpen += RechargePointOnOnOpen;
			}
		}
        private void Update()
        {
            if (startEncounter == true&& isAlreadyStarted == false)
            {
				Startup();
				isAlreadyStarted = true;
            }
        }

        private void RechargePointOnOnOpen()
		{
			CurrentHP = MaxHP;
			ResetPosition();
		}

		public void Startup()
		{
			_stateMachine.TrySwapState(BossState.Intro);

		}
		
		public void ResetPosition()
		{
			
			_stateMachine.TrySwapState(BossState.Dodge);
			
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
				_stateMachine.SwapState(BossState.Dead);
			}
		}

		private void JumpToRecharge(ObjectiveDoor objectiveDoor)
		{
			_mover.JumpToPosition(objectiveDoor.transform.position);
			_stateMachine.TrySwapState(BossState.Recharge);
		}

		public void SwitchToMelee()
		{
			_mover.TargetStoppingRange = this.TargetDistanceMelee;
		}

		public void SwitchToRange()
		{
			_mover.TargetStoppingRange = this.TargetDistanceRanged;
		}
	}
}