using UnityEngine;
using UnityEngine.SceneManagement;
using Environment;

namespace Management
{
    public class LevelLoader
    {
        public int _currentSceneIndex;
        public int _currentActiveScene;
        public static Scene hackingScene;

       
        private void OnLevelStart()
        {
            hackingScene = SceneManager.GetSceneByName("HackingScene");
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        public static void LoadHacking(HackingConsole hackingConsole)
        {
            SceneManager.LoadScene(hackingScene.name, LoadSceneMode.Additive);
           
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
            _currentActiveScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(_currentActiveScene++);
            OnLevelStart();
        }
        public void RestartScene()
        {
            _currentActiveScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(_currentSceneIndex);
        }

    }
}
