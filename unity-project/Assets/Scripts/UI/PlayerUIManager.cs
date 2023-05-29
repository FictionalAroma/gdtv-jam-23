using TMPro;
using UnityEngine;

namespace UI
{
	public class PlayerUIManager : MonoBehaviour
	{
		public static PlayerUIManager Instance { get; private set; }
		private void Awake() 
		{ 
			// If there is an instance, and it's not me, delete myself.
    
			if (Instance != null && Instance != this) 
			{ 
				Destroy(this); 
			} 
			else 
			{ 
				DontDestroyOnLoad(this);
				Instance = this; 
			} 
		}

		[field: SerializeField] public SliderDisplay PlayerHPSlider { get; private set; }
		[field: SerializeField] public TMP_Text Score { get; private set;}
	}
}
