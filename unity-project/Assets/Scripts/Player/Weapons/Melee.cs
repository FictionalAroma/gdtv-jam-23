using CommonComponents.Interfaces;
using System.Collections;
using UnityEngine;

namespace Player.Weapons
{
	public class Melee : BaseWeapon  
	{
		private Coroutine _punching;
		private Coroutine _smashing;
		[SerializeField] GameObject player;
		[SerializeField] AudioClip primaryMeleeSFX;
		[SerializeField] AudioClip secondaryMeleeSFX;
		[SerializeField] bool canPrimaryAttack = true;
		[SerializeField] Animator playerAnimator;
		[SerializeField] GameObject _secondaryAttackExplosion;
		private bool playerPrimaryAttacking = false;
		private bool isPrimaryAttacking = false;

		public override void BeginPrimaryAttack(Vector3 fireDirection)
		{
			FireDirection = fireDirection;
			playerPrimaryAttacking = true;
			if (!isPrimaryAttacking)
			{
				StartCoroutine(PunchingRepeater());
			}
			
		}
		public override void CancelPrimaryAttack(Vector3 lookDir)
		{
			playerPrimaryAttacking = false;
		}
		private IEnumerator PunchingRepeater()
		{
			playerPrimaryAttacking = true;
			while (playerPrimaryAttacking)
			{
				if (canPrimaryAttack)
                {
					canPrimaryAttack = false;
					var setup = weaponsSetup.primary;
					var primaryAttackCheck = GetNextBullet(setup, PrimaryShotPool);
					primaryAttackCheck.Initialize(transform.position + transform.forward, 0, setup.speed, setup.damage);
					//player.GetComponent<PlayerController>().jukeBox.PlayOneShot(primaryMeleeSFX);
					playerAnimator.SetInteger("primaryMeleePunchVaration", Random.Range(1, 3));
					playerAnimator.SetTrigger("primaryMeleePunch");
					primaryAttackCheck.GetComponent<CapsuleCollider>().radius = setup.timeToLive;
					yield return new WaitForSeconds(setup.cooldown);
					canPrimaryAttack = true;
				}
				yield return new WaitForEndOfFrame();

			}

			isPrimaryAttacking = false;
		}


		private bool holidingSecondary;
		private float holdTimer;
        [SerializeField] bool canSecondaryAttack;

        public override void BeginSecondaryAttack(Vector3 fireDirection,bool holding)
		{
			
			if (canSecondaryAttack)
            {
				playerAnimator.SetTrigger("isHoldingMelee");
				playerAnimator.ResetTrigger("isExplodingMelee");
				FireDirection = fireDirection;
				holidingSecondary = true;
				StartCoroutine(SmashingRepeater());
				canSecondaryAttack = false;
			}
			
		}
		public override void CancelSecondaryAttack(Vector3 lookDir)
		{

			if (holidingSecondary)
            {
				holidingSecondary = false;

				StopCoroutine(SmashingRepeater());
				var setup = weaponsSetup.secondary;
				var _secondaryAttackCheck = GetNextBullet(setup, SecondaryShotPool);
				_secondaryAttackCheck.Initialize(transform.position + transform.forward, 0, setup.timeToLive, setup.damage);
				var explosion = Instantiate(_secondaryAttackExplosion, (transform.position + transform.forward), Quaternion.identity);
				playerAnimator.ResetTrigger("isHoldingMelee");
				playerAnimator.SetTrigger("isExplodingMelee");
				//player.GetComponent<PlayerController>().jukeBox.PlayOneShot(secondaryMeleeSFX);
				_secondaryAttackCheck.GetComponent<CapsuleCollider>().radius = setup.speed;
				//_secondaryAttackCheck.GetComponent<ParticleSystem>().Play();
				Destroy(explosion, 1f);
				StartCoroutine(SecondaryAttackCooldown());
			}
		}

		private IEnumerator SmashingRepeater()
		{

            while (holidingSecondary)
			{

				holdTimer += Time.deltaTime;
				//var explosionPrefab = Instantiate(_explosionVFXPrefab);
				//explosionPrefab.transform.position = _secondaryAttackCheck.transform.position;
				

				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator SecondaryAttackCooldown()
		{
			Debug.Log("SecondaryAttackCooldown Initiated");
			yield return new WaitForSeconds(weaponsSetup.secondary.cooldown);
			canSecondaryAttack = true;
			Debug.Log(canSecondaryAttack);
		}


	}
}
