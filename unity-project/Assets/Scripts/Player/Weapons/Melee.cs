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
		public override void BeginPrimaryAttack(Vector3 fireDirection)
		{
			
			FireDirection = fireDirection;
			_punching = StartCoroutine(PunchingRepeater());
			
			
		}
		public override void CancelPrimaryAttack(Vector3 lookDir)
		{
			StopCoroutine(_punching);

		}
		private IEnumerator PunchingRepeater()
		{
			while (true)
			{
				if (canPrimaryAttack)
                {
					canPrimaryAttack = false;
					var setup = weaponsSetup.primary;
					var primaryAttackCheck = GetNextBullet(setup, PrimaryShotPool);
					primaryAttackCheck.Initialize(transform.position + transform.forward * setup.timeToLive, setup.timeToLive, setup.speed, setup.damage);
					player.GetComponent<PlayerController>().jukeBox.PlayOneShot(primaryMeleeSFX);
					player.GetComponent<Animator>().SetInteger("primaryMeleePunchVaration", Random.Range(1, 3));
					player.GetComponent<Animator>().SetTrigger("primaryMeleePunch");
					primaryAttackCheck.GetComponent<SphereCollider>().radius = setup.timeToLive;
					yield return new WaitForSeconds(setup.cooldown);
					canPrimaryAttack = true;
				}	
			}
		}
		
		public override void BeginSecondaryAttack(Vector3 fireDirection,bool holding)
		{
			Debug.Log("Start Smashing");
			FireDirection = fireDirection;
			_smashing = StartCoroutine(SmashingRepeater(holding));
		}
		public override void CancelSecondaryAttack(Vector3 lookDir)
		{
			StopCoroutine(_smashing);
			var setup = weaponsSetup.secondary;
			var _secondaryAttackCheck = GetNextBullet(setup, SecondaryShotPool);
			_secondaryAttackCheck.Initialize(transform.position + transform.forward * setup.timeToLive, setup.timeToLive, setup.speed, setup.damage);
			player.GetComponent<PlayerController>().jukeBox.PlayOneShot(secondaryMeleeSFX);
			_secondaryAttackCheck.GetComponent<SphereCollider>().radius = setup.timeToLive;
			_secondaryAttackCheck.GetComponent<ParticleSystem>().Play();

		}

		private IEnumerator SmashingRepeater(bool holding)
		{
			
			while (holding)
			{

				
				//var explosionPrefab = Instantiate(_explosionVFXPrefab);
				//explosionPrefab.transform.position = _secondaryAttackCheck.transform.position;
				

				yield return new WaitForEndOfFrame();
			}
		}

	}
}
