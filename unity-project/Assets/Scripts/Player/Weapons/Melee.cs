using CommonComponents.Interfaces;
using Management;
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
			isPrimaryAttacking = false;
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
					player.GetComponent<PlayerController>().jukeBox.PlayOneShot(primaryMeleeSFX);
					playerAnimator.SetInteger("primaryMeleePunchVaration", Random.Range(1, 3));
					player.GetComponent<Animator>().SetTrigger("primaryMeleePunch");
					primaryAttackCheck.GetComponent<SphereCollider>().radius = setup.timeToLive;
					yield return new WaitForSeconds(setup.cooldown);
					canPrimaryAttack = true;
				}
				yield return new WaitForEndOfFrame();

			}

			isPrimaryAttacking = false;
		}


		private bool holidingSecondary;
		private float holdTimer;
		public override void BeginSecondaryAttack(Vector3 fireDirection,bool holding)
		{
			Debug.Log("Start Smashing");
			FireDirection = fireDirection;
			holidingSecondary = true;
			StartCoroutine(SmashingRepeater());
		}
		public override void CancelSecondaryAttack(Vector3 lookDir)
		{
			StopCoroutine(_smashing);
			var setup = weaponsSetup.secondary;
			var _secondaryAttackCheck = GetNextBullet(setup, SecondaryShotPool);
			_secondaryAttackCheck.Initialize(transform.position + transform.forward, 0, setup.timeToLive, setup.damage);
			player.GetComponent<PlayerController>().jukeBox.PlayOneShot(secondaryMeleeSFX);
			_secondaryAttackCheck.GetComponent<SphereCollider>().radius = setup.timeToLive;
			_secondaryAttackCheck.GetComponent<ParticleSystem>().Play();

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

	}
}
