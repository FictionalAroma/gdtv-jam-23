using CommonComponents.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
	public class PauseMenuManager : MonoBehaviour
	{
		private static PauseMenuManager _instance;
		[SerializeField] private GameObject pauseScreen;

		private bool isPaused;
		private GameState _cachedState;

		public static PauseMenuManager Instance => _instance;

		void Start()
		{
			_instance = this;
			pauseScreen.SetActive(false);
		}

		public void OpenPauseMenu()
		{
			_cachedState = SingletonRepo.StateManager.CurrentState;

			SingletonRepo.StateManager.SetState(GameState.Paused);
			isPaused = true;
		}
		public void Resume()
		{
			SingletonRepo.StateManager.SetState(_cachedState);
			isPaused = false;

		}

		public void OnEnable()
		{
			pauseScreen.SetActive(true);
		}

		public void OnDisable()
		{
			pauseScreen.SetActive(false);
		}

		public void QuitApplication()
		{
			Application.Quit();
			Debug.Log("Escaping");
		}
		public void LoadMenu()
		{
			SceneManager.LoadScene("MainMenu");
			Resume();
		}


		//this is for other scripts potentially
		//i know theres probably loads of safer and simplers ways to do this but ehhhh
		public void TogglePause() 
		{
			if (isPaused)
			{
				Resume();
			}
			else
			{
				OpenPauseMenu();
			}
		}



	}
}
