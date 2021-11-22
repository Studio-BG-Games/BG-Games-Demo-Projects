using Mechanics.SketchBook;
using UnityEditor;

namespace DefaultNamespace
{
    public class ToolsForEditor
    {
        [MenuItem("Tools/Zero progress")]
        private static void ProgressToZero()
        {
            var dataFinish = new DataFinishedLevel();
            dataFinish.Clear();
            LogicPrometerOnSketchBook.ZeroMe();
        }
    }
}