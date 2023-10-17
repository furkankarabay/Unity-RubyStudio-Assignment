using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

namespace RubyGame.Managers
{
    public class LevelCanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelWinCanvas;
        [SerializeField] private GameObject levelFailedCanvas;
        [SerializeField] private TextMeshProUGUI infoPanel;

        private void OnEnable()
        {
            EventsManager.OnLevelCompleted += DisplayVictoryCanvas;
            EventsManager.OnLevelFailed += DisplayDefeatCanvas;
            StartCoroutine(FadeOutInfoPanel());
        }

        private void OnDisable()
        {
            EventsManager.OnLevelCompleted -= DisplayVictoryCanvas;
            EventsManager.OnLevelFailed -= DisplayDefeatCanvas;
        }

        IEnumerator FadeOutInfoPanel()
        {
            yield return new WaitForSecondsRealtime(5);
            infoPanel.DOFade(0, 1.25f);
        }

        private void DisplayVictoryCanvas()
        {
            levelWinCanvas.SetActive(true);
        }

        private void DisplayDefeatCanvas()
        {
            levelFailedCanvas.SetActive(true);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
