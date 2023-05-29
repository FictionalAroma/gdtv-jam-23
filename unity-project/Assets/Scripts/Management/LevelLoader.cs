using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
    public class LevelLoader : MonoBehaviour
    {
        public int _currentSceneIndex;
        public int _currentActiveScene;
       
        private void Start()
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        internal static void ExitGame()
        {
            Application.Quit();
        }
        public static void LoadStartScreen()
        {
            SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).name, LoadSceneMode.Single);
        }

        public static void GoToGameLevel()
        {
            SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).name, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics3D));
        }
        public void LoadNextScene()
        {
            _currentActiveScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(_currentActiveScene++);
        }
        public void RestartScene()
        {
            _currentActiveScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(_currentSceneIndex);
        }

    }
}
