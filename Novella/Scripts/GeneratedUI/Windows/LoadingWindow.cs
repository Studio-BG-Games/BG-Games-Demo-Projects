using Scripts.Managers;
using Scripts.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts;

namespace GeneratedUI
{
	public class LoadingWindow : WindowController
	{
		[SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI localizedLoadingDescription;
        [SerializeField] private TextMeshProUGUI localizedDisclaimer1;        
        [SerializeField] GameObject text, logo;

        private float fillSpeed = 0.35f, targetProgress = 0;
        private bool check;

        private void Start()
        {
            IncrementProgress(1f);
            GameManager.Instance.soundManager.DoNull(0.6f);
            switch (PlayerPrefs.GetString("language"))
            {
                case "rus":
                    localizedLoadingDescription.text = "   Наливаем коктейль...";
                    localizedDisclaimer1.text = "Все совпадения с реальными людьми случайны\nInfinite Games Inc";
                    break;
                case "eng":
                    localizedLoadingDescription.text = "Pouring a cocktail...";
                    localizedDisclaimer1.text = "All similarities are purely fictional\nInfinite Games Inc";
                    break;
            }
        }
        private void Update()
        {
            if (slider.value < targetProgress)
                slider.value += fillSpeed * Time.deltaTime;
            else if (!check)
            {
                check = true;
                CloseWindow();
            }
        }

        private void IncrementProgress(float newProgress)
        {
            targetProgress = slider.value + newProgress;
        }

        public void HideUnusedObjects()
        {
            text.SetActive(false);
            logo.SetActive(false);
        }
    }
}