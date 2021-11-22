using System;
using Infrastructure.Configs;
using Plugins.DIContainer;
using TMPro;
using UnityEngine;

namespace Mechanics
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LoaderFontForTMP_UI : MonoBehaviour
    {
        private void Awake() => GetComponent<TextMeshProUGUI>().font = DiBox.MainBox.ResolveSingle<ConfigLocalization>().FontLocal;
    }
}