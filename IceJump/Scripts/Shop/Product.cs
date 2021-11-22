using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
	[SerializeField] private string _nameSkin;
	[SerializeField] private int _price;
	[SerializeField] private Skin _skin;


	public string Name { get { return _nameSkin; } }
	public int Price { get{ return _price; } }
	public Skin Skin{ get{ return _skin; } set{ _skin = value; } }

	public bool isBuy = false;

	[SerializeField] private Text _textPrice;
	[SerializeField] private Image _spriteSkin;

	[SerializeField] private BuySkinButton _buyButton;
	[SerializeField] private SelectedSkinButton _selectedButton;


	private void Start()
	{
		_nameSkin = _skin.Name;
		_price = _skin.Price;
		_spriteSkin.sprite = _skin.SpriteSkin;
		_textPrice.text = _price.ToString();
		gameObject.name = "Product_" + _nameSkin;
		_selectedButton.OnSelectedSkinAction += OnSelectedSkin;
		_buyButton.OnBuySkinAction += OnBuySkin;

		CheckBuyProduct();
	}

	private void CheckBuyProduct()
	{
		if (_skin.isDefault)
		{
			isBuy = true;
			PlayerPrefs.SetInt("product_" + _nameSkin, 1);
			_buyButton.gameObject.SetActive(false);
			_selectedButton.gameObject.SetActive(true);
		}
		else
		{
			if (PlayerPrefs.HasKey("product_" + _nameSkin))
			{
				isBuy = true;
				if (PlayerPrefs.HasKey("skin_player"))
				{
					if (PlayerPrefs.GetString("skin_player") == _nameSkin)
					{
						GameHandler.Instance.SetSkin(_skin);
					}
				}
				_buyButton.gameObject.SetActive(false);
				_selectedButton.gameObject.SetActive(true);
			}

			else
			{
				isBuy = false;
				_buyButton.gameObject.SetActive(true);
				_selectedButton.gameObject.SetActive(false);
			}
		}
		
	}

	private void OnSelectedSkin()
	{
		GameHandler.Instance.SetSkin(_skin);
	}

	private void OnBuySkin()
	{
		if (ShopHandler.Instance.BuyProduct(this))
		{
			_selectedButton.gameObject.SetActive(true);
			_buyButton.gameObject.SetActive(false);
		}
	}
}
