using Management;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
	public class PlayerUIManager : MonoBehaviour
	{
		public static PlayerUIManager Instance { get; private set; }
		public float currentPlayerHealth;
		public LevelLoader levelLoader;
		Player.PlayerController playerController;
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

			playerController = FindAnyObjectByType<PlayerController>();
			currentPlayerHealth = playerController.MaxHP;
		}
        private void Update()
        {
			
            if (currentPlayerHealth <=0)
            {
				levelLoader.RestartScene();
				currentPlayerHealth = playerController.MaxHP;
				playerController.CurrentHP = playerController.MaxHP;
            }
        }


        [field: SerializeField] public SliderDisplay PlayerHPSlider { get; private set; }
		[field: SerializeField] public TMP_Text Score { get; private set;}
	}
}
