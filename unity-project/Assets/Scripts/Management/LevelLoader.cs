using System;
using CommonComponents.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Environment;
using Unity.VisualScripting;

namespace Management
{
    public class LevelLoader
    {
        public static int _currentSceneIndex;
        public static string _currentActiveScene;
        public static string hackingScene;
        GameStateManager gameStateManager;
		private static HackingConsole consoleCache;
		private void OnLevelStart()
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        public static void LoadHacking(HackingConsole console)
		{
			_currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			consoleCache = console;
			hackingScene = $"Hacking {console.hackingSceneDifficulty}";
			GameStateManager.Instance.SetState(CommonComponents.Interfaces.GameState.Hacking);

            SceneManager.LoadScene(hackingScene,LoadSceneMode.Additive);
        }
        public static void ExitHacking(bool playerWon)
		{
			if (playerWon)
			{
                consoleCache.TurnOnConsole();
			}
			else
			{
				SingletonRepo.PlayerObject.CurrentHP -= 10;
			}

			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneIndex));
			SceneManager.UnloadSceneAsync($"Hacking {consoleCache.hackingSceneDifficulty}");
            GameStateManager.Instance.SetState(GameState.Running);
		}
       
        internal static void ExitGame()
        {
            Application.Quit();
        }
        public static void LoadStartScreen()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public static void GoToGameLevel()
        {
            SceneManager.LoadScene(1);
			SceneManager.UnloadSceneAsync(0);
		}
        public static void LoadNextScene()
        {

            _currentActiveScene = SceneManager.GetActiveScene().name;
            if (_currentActiveScene == "Level 1")
            {
                SceneManager.LoadScene("Level 2");
            }
            if (_currentActiveScene == "Level 2")
            {
                SceneManager.LoadScene("Level 2");
            }
            if (_currentActiveScene == "Level 3")
            {
                SceneManager.LoadScene("MainMenu");
            }

			SceneManager.UnloadSceneAsync(_currentActiveScene);
			_currentActiveScene = SceneManager.GetActiveScene().name;

		}
        public void RestartScene()
        {
            _currentActiveScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(_currentActiveScene);
        }

    }
}
