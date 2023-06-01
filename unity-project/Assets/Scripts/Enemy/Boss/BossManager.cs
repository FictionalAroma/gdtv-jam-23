using System.Collections.Generic;
using CommonComponents;
using Enemy.States;
using Environment;
using UI;
using UnityEngine;

namespace Enemy.Boss
{
	[RequireComponent(typeof(UIDisplay))]
	[RequireComponent(typeof(EnemyStateMachine))]
	[RequireComponent(typeof(BossMover))]

	public class BossManager : Damagable
	{
		protected BossStateMachine _stateMachine;
		public Animator BossAnimator;
		protected BossMover _mover;

		public Collider[] _colliderCache;

		protected UIDisplay _hpBar;
		[SerializeField] private float TargetDistanceMelee;
		[SerializeField] private float TargetDistanceRanged;

		[SerializeField] private List<ObjectiveDoor> rechargePoints;
		[SerializeField] public Transform startPosition;
		public bool startEncounter;
		public bool isAlreadyStarted;
		protected override void Awake()
		{
			
			isAlreadyStarted = false;
			//startEncounter = false;
			_colliderCache = GetComponents<Collider>();
			base.Awake();

			_hpBar = GetComponent<UIDisplay>();
			
			HPChangedEvent += _hpBar.SetSliderValue;
			HPEmpty += OnDeath;

			_mover = GetComponent<BossMover>();

		}

		private void Start()
		{
			_stateMachine = GetComponent<BossStateMachine>();
			DamageTaken += _stateMachine.DamageTaken;
			_stateMachine.AddState(new BossIdleState(gameObject));
			_stateMachine.AddState(new BossAlertState(gameObject));
			_stateMachine.AddState(new BossAttackState(gameObject));
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
			_stateMachine.TrySwapState(BossState.Attack);

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