using Management;
using UnityEngine;

namespace UI
{
	public class MainMenuController : MonoBehaviour
	{
		[SerializeField] private Canvas mainMenuCanvas;
		[SerializeField] private Canvas creditsCanvas;
		[SerializeField] private Canvas optionsCanvas;
		public void StartGameClick()
		{
			LevelLoader.GoToGameLevel();
		}

		public void CreditsClick()
		{
			mainMenuCanvas.enabled = false;
			creditsCanvas.enabled = true;
		}

		public void BackClick()
		{
			mainMenuCanvas.enabled = true;
			creditsCanvas.enabled = false;
		}

		public void ExitGame()
		{
			LevelLoader.ExitGame();
		}

	}
}
