using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadPictures : ILoaderPictures
{
	private List<Picture> _pictures;

	public LoadPictures()
	{
		_pictures = new List<Picture>();
		Load();
	}
	public void Load()
	{
		var pictures = Resources.LoadAll<Picture>("ScriptableObjects/Pictures");
		_pictures = pictures.ToList();
	}

	public List<Picture> GetPictures()
	{
		return _pictures;
	}

	public List<Picture> GetPictureToCategory(Category category)
	{
		var pictures = _pictures.Where(picture => picture.Category == category);
		return pictures.ToList();
	}
}
