using CommonComponents.StateMachine;
using Enemy.States;

namespace Enemy.Boss
{
	public enum BossState
	{
		Idle,
		Intro,
		Recharge,
		MeleeAttack,
		RangedAttack,
		JumpAttack,
		ResetToCenter,
		Dead,
	}

    public class EnemyStateMachine : StateMachine<BossState, BossBaseState>
	{
		private BossStateContext _context;
        public BossBaseState GetCurrentState => CurrentState as BossBaseState;


		protected void Start()
		{
			_context = new BossStateContext()
					   {
				
						   Boss = GetComponent<BossManager>(),
						   PlayerCache = SingletonRepo.PlayerObject,
						   Attacker = GetComponent<BossMover>(),
					   };
		}

		public override bool AddState(BossBaseState newState)
		{
			var result = base.AddState(newState);
			newState.SetContext(_context);
			return result;

		}

		public override BossBaseState SwapState(BossState newStateEnum)
		{
			var oldState = GetCurrentState;
			oldState?.Deactivate();

			var newState = StateDictionary[newStateEnum];
			CurrentState = newState;

			_context.PlayerCache = SingletonRepo.PlayerObject;
			newState.SetContext(_context);
			newState.Activate();
			return newState;
		}

		public override BossBaseState TrySwapState(BossState newStateEnum)
		{
			return (BossBaseState)(CurrentState.State == BossState.Dead ? CurrentState : base.TrySwapState(newStateEnum));
		}

		public void DamageTaken(float amount)
		{
			var newStateEnum = GetCurrentState.DamageTaken(amount);
			TrySwapState(newStateEnum);
		}
	}
}