using TMPro;
using UnityEngine;

namespace Infrastructure.Configs
{
    [CreateAssetMenu(menuName = "Config/localization", order = 51)]
    public class ConfigLocalization : ScriptableObject
    {
        [Header("Шрифт локализации")]
        public TMP_FontAsset FontLocal;
        [Header("Меню")]
        [TextArea(1, 4)] public string Wellcome;
        [TextArea(1, 4)] public string FirstFixCar;
        [TextArea(1, 4)] public string HowToStartGame;
        [TextArea(1, 4)] public string SketchBookHello;
        [Header("Number Stage")]
        [TextArea(1, 4)] public string HelloNumberStage;
        [TextArea(1, 4)] public string HelloNumberStage2;
        [TextArea(1, 4)] public string EndNumberStage;
        [TextArea(1, 4)] public string PraiseNumber;
        [TextArea(1, 4)] public string NonPraiseNumber;
        [Header("Electro Stage")]
        [TextArea(1, 4)] public string TakeAnyWire;
        [TextArea(1, 4)] public string HelloElectroStage;
        [TextArea(1, 4)] public string CorrectSetWires;
        [TextArea(1, 4)] public string FailSetWires;
        [TextArea(1, 4)] public string FailElectroMove;
        [TextArea(1, 4)] public string StartTapeStage;
        [TextArea(1, 4)] public string FinishElectroStage;
        [Header("Canistro Stage")] 
        [TextArea(1, 4)] public string HelloCanistorStage;
        [TextArea(1, 4)] public string HelloCanistorStage2;
        [TextArea(1, 4)] public string ChooseAnyCanisters;
        [TextArea(1, 4)] public string СorrectlyChooseCanistro;
        [TextArea(1, 4)] public string FailChooseCanistro;
        [TextArea(1, 4)] public string EndCanistrostage;
        [Header("BossStage")]
        [TextArea(1,4)]public string HelloBossStage;
        [TextArea(1,4)]public string LoseBossStage;
        [TextArea(1,4)]public string WinBossStage;
    }
}