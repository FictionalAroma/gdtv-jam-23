using System.Collections.Generic;
using CommonComponents.StateMachine;
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
			FieldOfView fieldOfView = this._gameObject.GetComponent<FieldOfView>();
			return fieldOfView.canSeePlayer;
			/*var canSeePlayer = _gameObject.CanSeeTarget(Context.PlayerCache.gameObject,
														Context.EnemyManager.lookDistance,
														Context.EnemyManager.lookAngle,
														LayerMask.GetMask("Player", "Terrain", "PlayerBullets"),
														Context.PlayerCache.GetType(),
														true);
			RaycastHit raycastHit;
			var direction = (Context.PlayerCache.gameObject.transform.position - this._transform.position).normalized;
			if (Physics.Raycast(this._transform.position, direction, out raycastHit, Context.EnemyManager.lookDistance,LayerMask.GetMask("Player","Obstacles")))
            {
				if (raycastHit.transform.gameObject.CompareTag("Player"))
                {
					Debug.Log(Context.PlayerCache);
					Debug.Log("I see player");
					return true;
				}
				else
                {
					return false;
                }
				
            }
            else
            {
				return false;
            }*/
			
			
			
			
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
				var enemychecked = enemiesToCheck[i];
                if (enemychecked.transform.gameObject.TryGetComponent(out EnemyManager enemyManager))
                {
					RaycastHit raycastHit = new RaycastHit();
					var direction =(this._gameObject.transform.position - enemychecked.transform.position).normalized;
					if (Physics.Raycast(this._gameObject.transform.position, direction, out raycastHit, Context.EnemyManager.lookDistance, LayerMask.GetMask("Enemy", "Obstacles"), QueryTriggerInteraction.Collide))
					{
						Debug.Log(raycastHit.transform.gameObject.name);
						if (raycastHit.collider.gameObject.CompareTag("RangedEnemies") || raycastHit.collider.gameObject.CompareTag("BigEnemies"))
							enemiesToActivate.Add(enemychecked);
					}
				}
			
            }
			
			return enemiesToActivate;
		}
	}
}