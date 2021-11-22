using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "CoreGame/Config", order = 1)]
public class Configuration : ScriptableObject
{
	public AudioInstaller.Settings SettingsAudio;
	public GameInstaller.Settings SettingsGame;
}
