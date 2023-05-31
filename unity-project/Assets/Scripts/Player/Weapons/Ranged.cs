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
		[SerializeField] bool canPrimaryAttack = true;
		[SerializeField] bool canSecondaryAttack = true;
		Player.PlayerController Player;

		private bool playerIsAttacking;
		private bool primaryAttackRunning;
		//IInputInteraction interaction;
		


		// Start is called before the first frame update
		public override void BeginPrimaryAttack(Vector3 fireDirection)
		{
			Debug.Log("Start Firing");
			playerIsAttacking = true;
			if (!primaryAttackRunning)
			{
				StartCoroutine(FiringRepeater());
			}
		}

		public override void CancelPrimaryAttack(Vector3 lookDir)
		{
			playerIsAttacking = false;
		}

		private IEnumerator FiringRepeater()
		{
			primaryAttackRunning = true;
			while (playerIsAttacking)
			{
				if (canPrimaryAttack)
                {
					canPrimaryAttack = false;
					var setup = weaponsSetup.primary;
					var lazer = GetNextBullet(weaponsSetup.primary, PrimaryShotPool);

					lazer.Initialize(transform.position, setup.speed, setup.timeToLive, setup.damage);
					Player.GetComponent<Animator>().SetTrigger("primaryRangedShot");
					Player.GetComponent<Animator>().SetBool("isShooting", true);
					lazer.Fire(FireDirection.normalized);
					yield return new WaitForSeconds(setup.cooldown);
					canPrimaryAttack = true;
				}
				
			}
			primaryAttackRunning = false;

		}


		private bool chargingGrenade = false;
		private bool canThrowNext = true;
		public override void BeginSecondaryAttack(Vector3 fireDirection, bool interaction )
		{
			
			FireDirection = fireDirection;
			if (canThrowNext)
			{
				canThrowNext = false;

				chargingGrenade = true;
				_throwing = StartCoroutine(ThrowingGrenade());
			}

		}
		private IEnumerator ThrowingGrenade()
        {
			var setup = weaponsSetup.secondary;

			while (chargingGrenade)
			{
				throwingPower += Time.deltaTime * setup.speed;
				yield return new WaitForEndOfFrame();
			}

			throwingPower = Mathf.Clamp(throwingPower, weaponsSetup.primary.timeToLive, setup.timeToLive);
        }
		public override void CancelSecondaryAttack(Vector3 lookDir)
		{
			chargingGrenade = false;
			StopCoroutine(_throwing);
			var setup = weaponsSetup.secondary;

			var grenade = GetNextBullet(setup, SecondaryShotPool);
			Vector3 targetPosition = transform.position + transform.up * throwingPower;
			grenade.Initialize(grenade.transform.position, setup.speed * throwingPower, setup.speed, setup.damage);
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
			canThrowNext = true;
		}
    }
}
