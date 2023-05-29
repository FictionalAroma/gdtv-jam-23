using System;
using System.Collections.Generic;
using System.Linq;
using CommonComponents;
using UnityEngine;

namespace Environment
{
	public class ObjectiveDoor : MonoBehaviour
	{
		[SerializeField] private List<HackingConsole> keys;
		[SerializeField] private List<Damagable> keyEnemies;
		[SerializeField] private bool requireAllConditions;
		private bool _locked;
		private int startNumEnemies;

		public event Action OnOpen = null;

        public bool Locked { get => _locked; set => _locked = value; }

        private void Start()
		{
			foreach (var interactable in keys)
			{
				interactable.Subscribe(OnKeyChange);
			}

			if (keyEnemies != null)
			{
				foreach (var enemy in keyEnemies)
				{
					enemy.HPEmpty += OnKeyChange;
				}
			}

			startNumEnemies = keyEnemies.Count;
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
				Locked = false;
				this.gameObject.SetActive(false);
				OnOpen?.Invoke();
			}
		}

		private void OnKeyChange(Damagable health)
		{
			keyEnemies.Remove(health);
			OnKeyChange();
		}
	}
}