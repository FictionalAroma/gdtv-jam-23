using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
    public class LevelLoader
    {
        private int _currentSceneIndex;
        public static void LoadStartScreen()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public static void GoToMainGame()
		{
            SceneManager.LoadScene("Scenes/SampleScene", new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics3D));
        }

        private void Start()
        {
            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        internal static void ExitGame()
        {
            Application.Quit();
        }
    }
}
