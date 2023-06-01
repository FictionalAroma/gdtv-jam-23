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
		[SerializeField] Interactable Interactable;
		[SerializeField] private bool oneAndDone;
		public bool isActivated;
		private bool _locked;
		private int startNumEnemies;


		void Start()
		{
			coroutineIsCalled = false;
			trapEnabled = false;
			isActivated = false;
			trapCooldownTimer = trapCooldownTime;
			if (keys != null)
			{
				for(int index = keys.Count -1; index >= 0; index--)
				{
					var interactable = keys[index];
					if (interactable != null)
					{
						interactable.Subscribe(OnKeyChange);
					}
					else
					{
						keys.RemoveAt(index);
					}
				}
			}

			if (keyEnemies != null)
			{
				for(int index = keyEnemies.Count -1; index >= 0; index--)
				{
					var interactable = keyEnemies[index];
					if (interactable != null)
					{
						interactable.HPEmpty += OnKeyChange;
					}
					else
					{
						keyEnemies.RemoveAt(index);
					}
				}

				startNumEnemies = keyEnemies.Count;

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
			if (trapCooldownTimer <= 0 && coroutineIsCalled == false && oneAndDone == false)
            {
				ActivateTrapOnCooldown();
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
			if (oneAndDone)
            {
				StartCoroutine(ForceActivateTrapAndDisable());
            }
			else
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
		public  IEnumerator ForceActivateTrapAndDisable()
        {
			isActivated = true;
			yield return new WaitForSeconds(3);
			isActivated = false;
        }
	}
}
