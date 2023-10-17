using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RubyGame.UI
{
    public class PanelBehaviour : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject panelTopPanel;
        [SerializeField] private GameObject NextBtn;
        [SerializeField] private TextMeshProUGUI[] texts;
        [SerializeField] private Image panel;
        [SerializeField] private float openTime = .5f;

        [Range(0.01f, 1.0f)]
        [SerializeField] private float initialScaleAmount = 0.5f;

        private Vector3[] initialScales;
        private float[] initialAlphas;
        private float panelAlpha;

        #endregion

        #region Unity Events

        private void Awake()
        {
            SaveInitialStatus();
        }
        private void OnEnable()
        {
            OpenPanel();
        }

        #endregion

        #region Methods

        private void SaveInitialStatus()
        {
            int totalCount = texts.Length;
            initialScales = new Vector3[totalCount];
            initialAlphas = new float[totalCount];

            for (int i = 0; i < totalCount; i++)
            {
                int index = i;
                initialScales[i] = texts[index].rectTransform.localScale;
                initialAlphas[i] = texts[index].color.a;
            }

            panelAlpha = panel.color.a;
        }

        public void ScaleAndAlphaChange()
        {
            int totalCount = texts.Length;
            Color panelColor = panel.color;
            panelColor.a = 0;
            panel.color = panelColor;

            for (int i = 0; i < totalCount; i++)
            {
                int index = i;
                texts[index].rectTransform.localScale = initialScales[i] * initialScaleAmount;
                Color col = texts[index].color;
                col.a = 0;
                texts[index].color = col;
            }
        }

        public void OpenLeanTweens()
        {
            LeanTween.moveY(panelTopPanel.GetComponent<RectTransform>(), 0, openTime).setEase(LeanTweenType.easeOutBounce);

            int totalCount = texts.Length;
            for (int i = 0; i < totalCount; i++)
            {
                int index = i;
                LeanTween.scale(texts[index].gameObject, initialScales[i], openTime);
                LeanTween.value(0f, initialAlphas[i], openTime).setOnUpdate((float val) =>
                {
                    Color col = texts[index].color;
                    col.a = val;
                    texts[index].color = col;
                });
            }


            LeanTween.value(0f, panelAlpha, openTime).setOnUpdate((float val) =>
            {
                Color col = panel.color;
                col.a = val;
                panel.color = col;
            }).setOnComplete(() =>
            {
                panel.raycastTarget = true;
                NextBtn.gameObject.SetActive(true);
            });
        }

        public void OpenPanel()
        {
            panelTopPanel.GetComponent<RectTransform>().anchoredPosition += Vector2.up * 200;
            NextBtn.gameObject.SetActive(false);
            panel.raycastTarget = false;
            ScaleAndAlphaChange();
            OpenLeanTweens();
        }

        #endregion
    }
}
