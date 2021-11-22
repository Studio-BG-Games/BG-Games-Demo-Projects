using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollToProductHanlder : MonoBehaviour
{
	//Core
	private const string _pathToProductSkin = "ScriptableObjects/Products";

	[Header("__________________________________Product__________________________________")]
	[SerializeField] private Product _product;
	[SerializeField] private Skin[] _skins;
	[SerializeField] private List<Product> _products;

	[Space]

	[Header("__________________________________Content__________________________________")]
	[SerializeField] private Transform _content;
	private RectTransform _contentRectTransform;
	private int _selectedProductID;
	[SerializeField] private float _spacingPosition = 30f;
	[SerializeField] private float _spacingScale = 2f;
	[SerializeField] private float _smoothStepMove = 3f;
	[SerializeField] private float _smoothStepScale = 3f;
	[SerializeField] private float _maxSize = 1f;
	[SerializeField] private float _minSize = 0.5f; 
	[SerializeField] private List<Vector2> _productsPosition;
	[SerializeField] private Vector2[] _productsScale;
	[SerializeField] private Vector2 _contenVector;
	[SerializeField] private bool _isInit = false;
	[SerializeField] private bool _isScrolling = false;

	private void Start()
	{
		_contentRectTransform = _content.GetComponent<RectTransform>();
		_products = new List<Product>();
		_productsPosition = new List<Vector2>();
		LoadSkinsToResources();
		InitProducts();
		StartCoroutine(SnapToSelectedSkin());
	}

	private void FixedUpdate()
	{
		if (_isInit)
		{
			var nearestPosition = float.MaxValue;
			for (int i = 0; i < _skins.Length; i++)
			{
				float distance = Mathf.Abs(_contentRectTransform.anchoredPosition.x - _productsPosition[i].x);
				if (distance < nearestPosition)
				{
					nearestPosition = distance;
					_selectedProductID = i;
				}
				float _scale = Mathf.Clamp(1 / (distance / _spacingPosition) * _spacingScale, _minSize, _maxSize);
				_productsScale[i].x = Mathf.SmoothStep(_products[i].transform.localScale.x, _scale, _smoothStepScale);
				_productsScale[i].y = Mathf.SmoothStep(_products[i].transform.localScale.y, _scale, _smoothStepScale);
				_products[i].transform.localScale = _productsScale[i];
			}
			if (_isScrolling) return;
			_contenVector.x = Mathf.SmoothStep(_contentRectTransform.anchoredPosition.x, 
				_productsPosition[_selectedProductID].x, 
				_smoothStepMove * Time.fixedDeltaTime);
			_contentRectTransform.anchoredPosition = _contenVector;
			
		}
	}

	private IEnumerator SnapToSelectedSkin()
	{
		yield return new WaitForSeconds(0.2f);
		if(GameHandler.Instance.SkinPlayer != null)
		{
			foreach (var product in _products)
			{
				if (product.Name == GameHandler.Instance.SkinPlayer.Name)
				{
					_isInit = false;
					_contentRectTransform.anchoredPosition = -product.transform.localPosition;
					break;
				}
			}
			_isInit = true;
		}
	}

	private bool LoadSkinsToResources()
	{
		_skins = Resources.LoadAll<Skin>(_pathToProductSkin);
		if (_skins != null)
		{
			return true;
		}
		else
		{
			throw new System.Exception($"Not search product to path: {_pathToProductSkin}");
		}
	}

	private void InitProducts()
	{
		for (int i = 0; i < _skins.Length; i++)
		{
			_products.Add(Instantiate(_product, _content, false));
			_products[i].Skin = _skins[i];

			if (i > 0)
			{
				_products[i].transform.localPosition = new Vector2(_products[i - 1].transform.localPosition.x +
				_product.GetComponent<RectTransform>().sizeDelta.x +
				_spacingPosition, _products[i].transform.localPosition.y);
				_productsPosition.Add(-_products[i].transform.localPosition);
			}
			else
			{
				_productsPosition.Add(-_products[i].transform.localPosition);
			}
			

		}
		_productsScale = new Vector2[_skins.Length];
		_isInit = true;
	}

	public void Scrolling(bool isScroll)
	{
		_isScrolling = isScroll;
	}
}
