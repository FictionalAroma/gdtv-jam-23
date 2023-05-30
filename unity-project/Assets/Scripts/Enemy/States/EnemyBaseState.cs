using System.Collections.Generic;
using CommonComponents.StateMachine;
using Helpers;
using UnityEngine;

namespace Enemy.States
{
	public abstract class EnemyBaseState : BaseState<EnemyState>
	{
		protected EnemyStateContext Context;
		protected EnemyBaseState(GameObject obj, EnemyState state) : base(obj, state)
		{

		}

		public virtual EnemyState DamageTaken(float amount) => State;
		public void SetContext(EnemyStateContext context) => Context = context;

		protected bool CanSeePlayer()
		{
			var canSeePlayer = _gameObject.CanSeeTarget(Context.PlayerCache.gameObject,
														Context.EnemyManager.lookDistance,
														Context.EnemyManager.lookAngle,
														LayerMask.GetMask("Player", "Terrain", "PlayerBullets"),
														Context.PlayerCache.GetType(),
														true);
			return canSeePlayer;
		}

		protected IEnumerable<RaycastHit> GetEnemiesToActivate()
		{
			var enemiesToCheck = Physics.SphereCastAll(this._gameObject.transform.position,
														  Context.EnemyManager.lookDistance,
														  Vector3.forward,
														  1,
														  LayerMask.GetMask("Enemy"),
														  QueryTriggerInteraction.Collide);
			List<RaycastHit> enemiesToActivate = new List<RaycastHit>();
			
			for (int i =0; i<enemiesToCheck.Length; i++)
            {
				RaycastHit raycastHit = new RaycastHit();
				if (Physics.Raycast(this._gameObject.transform.position, enemiesToCheck[i].transform.position,out raycastHit, Context.EnemyManager.lookDistance, LayerMask.GetMask("Enemy", "Obstacles"), QueryTriggerInteraction.Collide))
				{
					if (raycastHit.collider.gameObject.CompareTag("RangedEnemies")|| raycastHit.collider.gameObject.CompareTag("BigEnemies"))
					enemiesToActivate.Add(enemiesToCheck[i]);
                }
				
					
            }
			
			return enemiesToActivate;
		}
	}
}