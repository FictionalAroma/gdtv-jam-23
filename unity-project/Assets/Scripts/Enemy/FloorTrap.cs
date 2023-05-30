using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonComponents;
using CommonComponents.Interfaces;
using Environment;
using UnityEngine;

namespace Enemy
{
	public class FloorTrap : MonoBehaviour , IDamageDealer
	{
		public float Damage => throw new System.NotImplementedException();
		BoxCollider boxCollider;
		public bool trapEnabled;
		[SerializeField] GameObject flameVFX;
		[SerializeField] private List<HackingConsole> keys;
		[SerializeField] private List<Damagable> keyEnemies;
		[SerializeField] private bool requireAllConditions;
		[SerializeField] private float trapCooldownTime;
		[SerializeField] private float trapCooldownTimer;
		[SerializeField] private bool stopCooldownTimer;
		[SerializeField] private float trapActiveTime;
		[SerializeField] private bool coroutineIsCalled;
		[SerializeField] public bool forceTrapActivation;
		public bool isActivated;
		private bool _locked;
		private int startNumEnemies;


		void Start()
		{
			forceTrapActivation = false;
			coroutineIsCalled = false;
			trapEnabled = false;
			isActivated = false;
			trapCooldownTimer = trapCooldownTime;
			foreach (var interactable in keys)
			{
				interactable.Subscribe(OnKeyChange);
			}

			foreach (var enemy in keyEnemies)
			{
				enemy.HPEmpty += OnKeyChange;
			}

			startNumEnemies = keyEnemies.Count;
			boxCollider = GetComponent<BoxCollider>();
			boxCollider.enabled = false;
			flameVFX.SetActive(false);
		}

		// Update is called once per frame
		void Update()
		{
			if (stopCooldownTimer == false)
            {
				trapCooldownTimer -= Time.deltaTime;
            }
			if (trapCooldownTimer <= 0 && coroutineIsCalled == false && forceTrapActivation == false)
            {
				ActivateTrapOnCooldown();
            }
			if (forceTrapActivation)
            {
				isActivated = true;
            }
			if (isActivated)
			{
				boxCollider.enabled = true;
				flameVFX.SetActive(true);

			}
			else
			{
				boxCollider.enabled = false;
				flameVFX.SetActive(false);
			}
		}
	
		private void OnKeyChange()
		{
			bool canUnlock = false;
			if (keys.Count > 0)
			{
				canUnlock |= keys.All(console => console.ActiveState);
			}

			if (startNumEnemies > 0 && (requireAllConditions || keys.Count == 0))
			{
				canUnlock |= keyEnemies.Count == 0;
			}

			if (canUnlock)
			{
				_locked = false;
				this.gameObject.SetActive(false);
			}
		}

		private void OnKeyChange(Damagable health)
		{
			keyEnemies.Remove(health);
			OnKeyChange();
		}
		public IEnumerator ActivateTrapOnCooldown()
        {
			coroutineIsCalled = true;
			trapActiveTime = Random.Range(1, 3);
			trapEnabled = true;
			stopCooldownTimer = true;
			yield return new WaitForSeconds(trapActiveTime);
			stopCooldownTimer = false;
			trapEnabled = false;
			trapCooldownTimer = trapCooldownTime;
			coroutineIsCalled = false;
        }
	}
}
