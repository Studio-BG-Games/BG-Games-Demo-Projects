using UnityEngine;

namespace Metrica
{
    public class SendYandexEvent : MonoBehaviour
    {
        [SerializeField] private string _nameEvent;
        
        public void Send() => AppMetrica.Instance.ReportEvent(_nameEvent);
    }
}