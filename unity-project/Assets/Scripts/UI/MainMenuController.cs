using Management;
using UnityEngine;

namespace UI
{
	public class MainMenuController : MonoBehaviour
	{
		public void StartGameClick()
		{
			LevelLoader.GoToMainGame();
		}

		public void ExitGame()
		{
			LevelLoader.ExitGame();
		}

	}
}
