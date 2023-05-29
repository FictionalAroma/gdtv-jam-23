using System.Collections.Generic;
using System.Linq;
using CommonComponents;
using CommonComponents.Interfaces;
using Environment;
using UnityEngine;

namespace Enemy
{
	public class OscillatorTrap : MonoBehaviour , IDamageDealer
	{
		Vector3 startingPosition;
		[SerializeField] Vector3 movementVector;
		[SerializeField] float period = 2f;
		[SerializeField] private List<HackingConsole> keys;
		//[SerializeField] private List<Damagable> keyEnemies;
		[SerializeField] private bool requireAllConditions;
		private bool _locked;
		[field: SerializeField] public float Damage { get; set; }
		//private int startNumEnemies;

		float movementFactor;

    

		// Start is called before the first frame update
		void Start()
		{
			foreach (var interactable in keys)
			{
				interactable.Subscribe(OnKeyChange);
			}

			/*foreach (var enemy in keyEnemies)
		{
			enemy.HPEmpty += OnKeyChange;
		}*/

			//startNumEnemies = keyEnemies.Count;
			startingPosition = transform.position;

		}

		// Update is called once per frame
		void Update()
		{
			if (period <= Mathf.Epsilon) return;
			float cycles = Time.time / period; // growing over time
			const float tau = Mathf.PI * 2; // constant value 6.283
			float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1
			movementFactor = (rawSinWave + 1f) / 2f; // recalculated to go from 0 to 1
			Vector3 offset = movementVector * movementFactor;
			transform.position = startingPosition + offset;
		}
	

		private void OnKeyChange()
		{
			bool canUnlock = false;
			if (keys.Count > 0)
			{
				canUnlock |= keys.All(console => console.ActiveState);
			}

			/*if (startNumEnemies > 0 && (requireAllConditions || keys.Count == 0))
		{
			canUnlock |= keyEnemies.Count == 0;
		}*/

			if (canUnlock)
			{
				_locked = false;
				this.gameObject.SetActive(false);
			}
		}

		private void OnKeyChange(Damagable health)
		{
			//keyEnemies.Remove(health);
			OnKeyChange();
		}
    
    
	}
}
