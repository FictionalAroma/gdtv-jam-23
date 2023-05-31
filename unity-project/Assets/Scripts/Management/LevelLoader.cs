using UnityEngine;
using UnityEngine.SceneManagement;
using Environment;

namespace Management
{
    public class LevelLoader
    {
        public int _currentSceneIndex;
        public string _currentActiveScene;
        public static string hackingScene;
        GameStateManager gameStateManager;
       
        private void OnLevelStart()
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        public static void LoadHacking(HackingConsole hackingConsole)
        {
            
            hackingScene = hackingConsole.hackingSceneDifficulty;
            SceneManager.LoadScene(hackingScene,LoadSceneMode.Additive);
            GameStateManager.Instance.SetState(CommonComponents.Interfaces.GameState.Hacking);
            


        }
        public static void ExitHacking(bool playerWon)
        {

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
            SceneManager.LoadScene(1, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics3D));
        }
        public void LoadNextScene()
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
            
        }
        public void RestartScene()
        {
            _currentActiveScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(_currentActiveScene);
        }

    }
}
