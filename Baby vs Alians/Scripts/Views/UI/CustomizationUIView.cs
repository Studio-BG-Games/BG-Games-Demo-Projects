using UnityEngine;
using UnityEngine.UI;

public class CustomizationUIView : MonoBehaviour
{
    #region Fields

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _shirtNextButton;
    [SerializeField] private Button _shirtPrevButton;
    [SerializeField] private Button _hairNextButton;
    [SerializeField] private Button _hairtPrevButton;
    [SerializeField] private Button _gunNextButton;
    [SerializeField] private Button _gunPrevButton;

    #endregion


    #region Properties

    public Button StartButton => _startButton;
    public Button BackButton => _backButton;
    public Button ShirtNextButton => _shirtNextButton;
    public Button ShirtPrevButton => _shirtPrevButton;
    public Button HairNextButton => _hairNextButton;
    public Button HairPrevButton => _hairtPrevButton;
    public Button GunNextButton => _gunNextButton;
    public Button GunPrevButton => _gunPrevButton;

    #endregion
}
