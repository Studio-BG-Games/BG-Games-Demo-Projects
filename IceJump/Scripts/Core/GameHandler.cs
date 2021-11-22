using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
	public static GameHandler Instance;
	[SerializeField] private int _poolCount = 2;
	[SerializeField] private bool _autoExpand = false;
	[SerializeField] private List<Level> _levelVariantPrefab;
	[SerializeField] private Transform _gameTransformPrefab;
	private Transform _gameTransform;
	private List<MonoPool<Level>> _pools;

	[SerializeField] private Player _player;
	public Skin SkinPlayer;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	public void SetSkin(Skin skin)
	{
		SkinPlayer = skin;
		PlayerPrefs.SetString("skin_player", skin.Name);
	}
	public void StartGame()
	{
		
		_gameTransform = Instantiate(_gameTransformPrefab);
		_player = Player.Instance;
		_player.Restart();
		_player.transform.position = _player.StartPosition;
		if(SkinPlayer != null)
		{
			_player.SetSkin(SkinPlayer);
		}
		
		_pools = new List<MonoPool<Level>>();
		for (int i = 0; i < _poolCount; i++)
		{
			var pool = new MonoPool<Level>(_levelVariantPrefab[i], _poolCount, _gameTransform);
			_pools.Add(pool);
			_pools[i].IsAutoExpand = _autoExpand;
		}
		CreateLevel();


		_player.gameObject.SetActive(true);
	}

	public void Restart()
	{
		_pools = new List<MonoPool<Level>>();
		Destroy(_gameTransform.gameObject);
		_gameTransform = null;

	}

	[SerializeField] private Level _destroyLevel;
	public void CreateLevel()
	{
		foreach (var pool in _pools)
		{
			if (pool.HasFreeElement(element: out var element))
			{
				if(_destroyLevel != null && element.Id == _destroyLevel.Id)
				{
					element.gameObject.SetActive(false);
					continue;
				}
				var level = element;
				level.TriggerEnd.SetActive(false);
				if (_destroyLevel != null)
				{
					level.transform.position = new Vector3(_destroyLevel.PointUp.position.x, _destroyLevel.PointUp.position.y + 3, _destroyLevel.PointUp.position.z);
				}
				level.gameObject.SetActive(true);
				break;
			}
		}


	}

	public void DestroyLevel()
	{
		if(_destroyLevel != null)
		{
			_destroyLevel.gameObject.SetActive(false);
			foreach (var poolBusyElement in _pools)
			{
				if (poolBusyElement.HasBusyElement(element: out var elementBusy))
				{
					_destroyLevel = elementBusy;
					_destroyLevel.TriggerEnd.SetActive(true);
				}
			}
		}
		else
		{
			foreach (var poolBusyElement in _pools)
			{
				if (poolBusyElement.HasBusyElement(element: out var elementBusy))
				{
					_destroyLevel = elementBusy;
					_destroyLevel.TriggerEnd.SetActive(true);
					//elementBusy.gameObject.SetActive(false);
				}
			}
		}
		
	}

}
