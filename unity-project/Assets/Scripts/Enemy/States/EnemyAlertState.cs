using UnityEngine;

namespace Enemy.States
{
	public class EnemyAlertState : EnemyBaseState
	{
		private float _alertActivationTime = 2.5f;
		private float _alertTimer = 2.5f;

		private float _alertResetTime = 5f;
		private float _alertReset = 5f;
		private Vector3 _originalPosition;

		private float _attackGraceTimer = 5f;


		public Transform lookTarget { get; set; }
		public EnemyAlertState(GameObject obj, EnemyState state = EnemyState.Alert) : base(obj, state)
		{

		}

		public override EnemyState Tick()
		{

			var canSeePlayer = CanSeePlayer();

			if (canSeePlayer)
			{
				_alertReset = _alertResetTime;
				_alertTimer -= Time.deltaTime;
				if (_alertTimer < 0f)
				{
					return EnemyState.Attack;
				}
			}
			else
			{
				_alertTimer = _alertActivationTime;
				_alertReset -= Time.deltaTime;
				if (_alertReset < 0)
				{
					Context.Attacker.SetPosition(_originalPosition);
				}
			}


			return State;
		}

		public override void Activate()
		{
			_originalPosition = _gameObject.transform.position;

			lookTarget = Context.PlayerCache.transform;
			Context.Attacker.moveTo = true;
			Context.Attacker.Target = lookTarget;
			Context.Attacker.StartShooting = false;

			var enemiesToActivate = GetEnemiesToActivate();
			foreach (var hit in enemiesToActivate)
			{
				if(hit.transform == this._transform) continue;
				if (hit.collider.TryGetComponent<EnemyStateMachine>(out var mach))
				{
					var state = mach.GetCurrentState.State;
					if(state == EnemyState.Alert || state == EnemyState.Attack) continue;
				}
				if (hit.collider.TryGetComponent<EnemyAttacker>(out var move))
				{
					move.Target = this._gameObject.transform;
					move.moveTo = false;
					move.StartShooting = false;
				}
			}
		}

		public override void Deactivate() {}

		public override EnemyState DamageTaken(float amount)
		{
			Context.Attacker.Target = Context.PlayerCache.transform;
			return _attackGraceTimer < 0 ? EnemyState.Attack : State;
		}
	}
}