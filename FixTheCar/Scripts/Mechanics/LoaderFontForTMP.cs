using Infrastructure.Configs;
using Plugins.DIContainer;
using TMPro;
using UnityEngine;

namespace Mechanics
{
    [RequireComponent(typeof(TextMeshPro))]
    public class LoaderFontForTMP : MonoBehaviour
    {
        private void Awake() => GetComponent<TextMeshPro>().font = DiBox.MainBox.ResolveSingle<ConfigLocalization>().FontLocal;
    }
}