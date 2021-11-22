using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausedHandler : MonoBehaviour
{
	public static PausedHandler Instance;
	[SerializeField] private GameObject _mapLevels;
	[SerializeField] private GameObject _buttonBack;
	[SerializeField] private GameObject _buttonHome;


	[SerializeField] private GameObject _winPanel;
	[SerializeField] private GameObject _homePanel;
	[SerializeField] private Text _textCurrentCompletLevel;
	[SerializeField] private GameObject _popupHelper;
	[SerializeField] private Text _textPopup;

	[SerializeField] private GameObject _settingsPanel;
	[SerializeField] private Slider _sliderSound;
	[SerializeField] private Text _textSoundValue;
	[SerializeField] private Slider _sliderMusic;
	[SerializeField] private Text _textMusicValue;
	private bool isSettings = false;

	public Text _textCountMoleculesReward;
	private const string _sceneMenu = "_Menu";

	private void Awake()
	{
		Instance = this;
		_sliderSound.value = AudioHandler.Instance.SoundSource.volume;
		_sliderMusic.value = AudioHandler.Instance.MusicSource.volume;
	}

	public void Update()
	{
		if (isSettings)
		{
			AudioHandler.Instance.SoundSource.volume = _sliderSound.value;
			_textSoundValue.text = _sliderSound.value * 100 + "%";

			AudioHandler.Instance.MusicSource.volume = _sliderMusic.value;
			_textMusicValue.text = _sliderMusic.value * 100 + "%";
		}
	}
	public void ActiveBoxColliderToCones(bool active)
	{
		if (GameHandler.Instance.CurrentLevel != null)
		{
			foreach (var cone in GameHandler.Instance.CurrentLevel.Cones)
			{
				cone.BoxCollierd2D.enabled = active;
			}
		}


	}
	public void ContinueToMenu()
	{
		SceneManager.LoadScene(_sceneMenu);
	}
	public void OpenMapLevels(bool isHome = false)
	{
		if (isHome)
		{
			_buttonBack.SetActive(false);
			_buttonHome.SetActive(true);
		}
		else
		{
			_buttonBack.SetActive(true);
			_buttonHome.SetActive(false);

		}
		_mapLevels.SetActive(true);
		ActiveBoxColliderToCones(false);
	}
	public void CloseMapLevels(bool isHome = false)
	{
		if (isHome)
		{
			_buttonBack.SetActive(true);
			_buttonHome.SetActive(false);
		}
		ActiveBoxColliderToCones(true);
		_mapLevels.SetActive(false);
	}

	public void OpenSettingsPanel()
	{
		isSettings = true;
		_settingsPanel.SetActive(true);
	}

	public void CloseSettingsPanel()
	{
		isSettings = false;
		_settingsPanel.SetActive(false);
	}
	public void OpenWinPanel()
	{
		ActiveBoxColliderToCones(false);
		_winPanel.SetActive(true);
		_textCountMoleculesReward.text = GameHandler.Instance.CurrentLevel.RewardMolecules.ToString();
		_textCurrentCompletLevel.text = $"спнбемэ { GameHandler.Instance.CurrentLevel.Id}\nопнидем";
	}
	public void CloseWinPanel()
	{
		ActiveBoxColliderToCones(true);
		_winPanel.SetActive(false);
	}

	public void CloseHomePanel()
	{
		ActiveBoxColliderToCones(true);
		_homePanel.SetActive(false);
	}

	public void ShowPopupHelper(string comment)
	{
		StopAllCoroutines();
		_popupHelper.SetActive(false);
		_textPopup.text = comment;
		StartCoroutine(ShowPopup());
	}

	private IEnumerator ShowPopup()
	{
		_popupHelper.SetActive(true);
		yield return new WaitForSeconds(5.5f);
		_popupHelper.SetActive(false);

	}
}
