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
		[SerializeField] private GameObject[] doorsToOpen;
		[SerializeField] private Collider colliderToDisable;
		private bool _locked;
		private int startNumEnemies;

		public event Action OnOpen = null;

        public bool Locked { get => _locked; set => _locked = value; }

        private void Start()
		{
			_locked = true;
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
				for (int i = 0; i<doorsToOpen.Length; i++)
                {
					doorsToOpen[i].SetActive(false);
					colliderToDisable.enabled = false;
				}
				
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