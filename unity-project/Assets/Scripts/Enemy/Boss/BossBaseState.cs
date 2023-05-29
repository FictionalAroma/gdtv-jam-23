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


		protected IEnumerable<RaycastHit> GetEnemiesToActivate()
		{
			var enemiesToActivate = Physics.SphereCastAll(this._gameObject.transform.position,
														  Context.EnemyManager.lookDistance,
														  Vector3.forward,
														  1,
														  LayerMask.GetMask("Enemy"),
														  QueryTriggerInteraction.Collide);
			
			return enemiesToActivate;
		}
	}
}