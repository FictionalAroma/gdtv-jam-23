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
		private bool _locked;
		private int startNumEnemies;


		void Start()
		{
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
			if (trapEnabled)
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
	}
}
