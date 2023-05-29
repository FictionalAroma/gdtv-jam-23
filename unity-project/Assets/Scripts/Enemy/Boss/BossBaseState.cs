using System.Collections.Generic;
using CommonComponents.StateMachine;
using UnityEngine;

namespace Enemy.Boss
{
	public abstract class BossBaseState : BaseState<BossState>
	{
		protected BossStateContext Context;
		protected BossBaseState(GameObject obj, BossState state) : base(obj, state)
		{

		}

		public virtual BossState DamageTaken(float amount) => State;
		public void SetContext(BossStateContext context) => Context = context;


	
	}
}