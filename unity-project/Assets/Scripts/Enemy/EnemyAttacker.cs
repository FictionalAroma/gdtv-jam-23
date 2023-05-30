using CommonComponents;
using Player.Weapons;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttacker : EnemyMover
	{
		[SerializeField] private WeaponMode weaponSetup;
		[SerializeField] private float takeShotRange;
		Animator animator;
		[SerializeField] private bool StopMovingAtRange;
		[SerializeField] private bool FireIfNotInRange;

		public bool StartShooting { get; set; } = true;

		private float _shotCooldown;

		private ObjectCachePool<Projectile> _bulletPool;

		protected override void Awake()
		{
			base.Awake();
			animator = GetComponent<EnemyManager>().enemyAnimator;
			_bulletPool = new ObjectCachePool<Projectile>(weaponSetup.projectile, 5);
		}


		protected override void Update()
		{
			_moveAdjustmentTimer -= Time.deltaTime;
			_shotCooldown -= Time.deltaTime;
			if (!hasTarget || Target == null || !Target.gameObject.activeInHierarchy) return;

			bool inRange = Vector3.Distance(transform.position, Target.position) < takeShotRange;

			this.transform.rotation = Quaternion.LookRotation(Target.position - this.transform.position);

			if (StartShooting && _shotCooldown < 0f && (inRange || FireIfNotInRange))
			{
				var bullet = _bulletPool.PullObject();
				var position = transform.position;
				bullet.Initialize(position, weaponSetup.speed, weaponSetup.timeToLive, weaponSetup.damage);
				animator.SetBool("isRunning", false);
				animator.SetInteger("meleeAttackVaration", Random.Range(0, 2));
				animator.SetTrigger("isShooting");
				bullet.Fire((Target.position - position).normalized);
				_shotCooldown = weaponSetup.cooldown;
			}

			if (moveTo && _moveAdjustmentTimer < 0f && (!inRange || !StopMovingAtRange) )
			{
				animator.SetBool("isRunning", true);
				SetNavDestination(Target.position);
				_navMeshAgent.stoppingDistance = StopMovingAtRange ? takeShotRange : 0;
			}

			
		}
	}
}