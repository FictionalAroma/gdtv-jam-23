using System.Collections;
using CommonComponents.Interfaces;
using UnityEngine;

namespace Player.Weapons
{
	public class Ranged : BaseWeapon
	{
		private Coroutine _firing;
		private Coroutine _throwing;
		public float throwingPower;
		[SerializeField] AudioClip primaryRangedSFX;
		[SerializeField] AudioClip secondaryRangedSFX;
		//IInputInteraction interaction;
		


		// Start is called before the first frame update
		public override void BeginPrimaryAttack(Vector3 fireDirection)
		{
			Debug.Log("Start Firing");
			FireDirection = fireDirection;
			_firing = StartCoroutine(FiringRepeater());
		}

		public override void CancelPrimaryAttack(Vector3 lookDir)
		{
			StopCoroutine(_firing);

		}

		private IEnumerator FiringRepeater()
		{
			while (true)
			{
				var setup = weaponsSetup.primary;
				var lazer = GetNextBullet(weaponsSetup.primary, PrimaryShotPool);

				lazer.Initialize(transform.position, setup.speed, setup.timeToLive, setup.damage);

				lazer.Fire(FireDirection.normalized);
				yield return new WaitForSeconds(setup.cooldown);
			}
		}

		public override void BeginSecondaryAttack(Vector3 fireDirection, bool interaction )
		{
			
			
			FireDirection = fireDirection;
			_throwing = StartCoroutine(ThrowingGrenade(interaction));
			
		}
		private IEnumerator ThrowingGrenade(bool holding)
        {
			var setup = weaponsSetup.secondary;

			while (holding)
			{
				throwingPower += Time.deltaTime * setup.speed;
				yield return new WaitForEndOfFrame();
			}

            Mathf.Clamp(throwingPower, weaponsSetup.primary.timeToLive, setup.timeToLive);
			CancelSecondaryAttack(FireDirection);
        }
		public override void CancelSecondaryAttack(Vector3 lookDir)
		{
			StopCoroutine(_throwing);
			var setup = weaponsSetup.secondary;

			var grenade = GetNextBullet(setup, SecondaryShotPool);
			Vector3 targetPosition = transform.position + transform.up * throwingPower;
			grenade.Initialize(grenade.transform.position, setup.speed, setup.speed, setup.damage);
			Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
			
			grenadeRigidbody.velocity = CalculateThrowVelocity(grenade.transform.position, targetPosition, 1.5f); // Adjust the multiplier as desired

			StartCoroutine(SecondaryAttackCooldown());
		}

		private Vector3 CalculateThrowVelocity(Vector3 start, Vector3 target, float timeMultiplier)
		{
			float maxHeight = 5f; // Adjust this value to control the maximum height of the throw
			float timeToTarget = Mathf.Sqrt( 0.5f * maxHeight / Mathf.Abs(Physics.gravity.y)) / timeMultiplier; // Adjust the timeToTarget using the multiplier
			float distanceToTarget = Vector3.Distance(start, target);
			Vector3 velocityY = Vector3.up * Mathf.Abs(Physics.gravity.y) * timeToTarget;
			Vector3 velocityXZ = (target - start) / timeToTarget;

			return velocityXZ + velocityY;
		}



		private IEnumerator SecondaryAttackCooldown()
        {			
			throwingPower = weaponsSetup.primary.timeToLive;
			yield return new WaitForSeconds(weaponsSetup.secondary.cooldown);
        }
    }
}
