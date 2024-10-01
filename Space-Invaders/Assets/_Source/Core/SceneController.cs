using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneController
    {
        public bool IsPaused { get; private set; }
        
        public void ReloadScene()
        {
            PauseGame();
            SceneManager.LoadScene("SampleScene");
            ResumeGame();
        }
        public void PauseGame()
        {
            Time.timeScale = 0f;
            IsPaused = true;
        }
        public void ResumeGame()
        {
            Time.timeScale = 1f;
            IsPaused = false;
        }
    }
}