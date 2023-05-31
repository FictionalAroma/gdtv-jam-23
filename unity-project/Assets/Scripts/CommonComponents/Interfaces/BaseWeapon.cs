using Player.Weapons;
using UnityEngine;

namespace CommonComponents.Interfaces
{
	public abstract class BaseWeapon : MonoBehaviour
	{
		public Vector3 FireDirection { get; set; }

        [SerializeField] protected WeaponsSetup weaponsSetup;
		 protected ObjectCachePool<Projectile> PrimaryShotPool;
		 protected ObjectCachePool<Projectile> SecondaryShotPool;

		public void Awake()
		{
			PrimaryShotPool = new ObjectCachePool<Projectile>(weaponsSetup.primary.projectile, 30);
			SecondaryShotPool = new ObjectCachePool<Projectile>(weaponsSetup.secondary.projectile, 30);
		}

		private void OnDestroy()
		{
			PrimaryShotPool.Destroy();
			SecondaryShotPool.Destroy();
		}

		public abstract void BeginPrimaryAttack(Vector3 fireDirection);
		public abstract void BeginSecondaryAttack(Vector3 fireDirection, bool interaction);
		public virtual void CancelPrimaryAttack(Vector3 lookDir) { }
		public virtual void CancelSecondaryAttack(Vector3 lookDir) { }

		public Projectile GetNextBullet(WeaponMode mode , ObjectCachePool<Projectile> objectCachePool)
		{
			var shot = objectCachePool.PullObject();
			shot.Initialize(transform.position, mode.speed, mode.timeToLive, mode.damage);
			return shot;
		}
	}
}