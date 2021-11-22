using System.Collections.Generic;
public interface ILoaderPictures
{
	public void Load();
	public List<Picture> GetPictures();
	public List<Picture> GetPictureToCategory(Category category);
}
