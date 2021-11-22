using System.Collections;
using Mechanics.GameLevel.Stages.BossStagePart.Interfaces;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mechanics.GameLevel.Stages.BossStagePart
{
    [RequireComponent(typeof(Collider2D))]
    public class Valve : MonoBehaviour, IPointerReciver, IRestartable, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Pipe _pipe;
        [SerializeField] private SpriteValveSwitch _spriteValveSwitch;
        [RequireInterface(typeof(IBossPart))][SerializeField] private Object _bossPart;
        [SerializeField][Min(0.1f)] private float _valueTarger;
        [SerializeField]private float _speedRotateSprite;

        private float _currentProgress=0;
        private IBossPart BossPart=>_bossPart as IBossPart;
        private Collider2D _collider2D;
        private Coroutine _actionMakeProgress;
        private Player _player;

        [DI]
        private void Init() => _collider2D = GetComponent<Collider2D>();
        
        public void On() => _collider2D.enabled = true;

        public void Off() => _collider2D.enabled = false;
        
        public void Restart()
        {
            _spriteValveSwitch.ChangeToUnactive();
            BossPart.MakeDirty();
            _currentProgress = 0;
        }

        public void OnPointerExit(PointerEventData eventData) => StopProgres();

        private IEnumerator MakeProgress()
        {
            while (_currentProgress<_valueTarger)
            {
                _currentProgress += Time.deltaTime;
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 1) * _speedRotateSprite;
                yield return null;
                if (_currentProgress >= _valueTarger)
                    Finish();
            }
        }

        private void Finish()
        {
            _spriteValveSwitch.ChangeToActie();
            _pipe.MakeFlow(() => BossPart.MakeClear());
        }

        public void OnPointerUp(PointerEventData eventData) => StopProgres();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Player>(out var player)) _player = player;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Player>(out var player))
            {
                StopProgres();
                _player = null;
            }
        }

        private void StopProgres()
        {
            if (_actionMakeProgress != null)
            {
                _player.MakeFakeStopMove(false);
                StopCoroutine(_actionMakeProgress);
                _actionMakeProgress = null;
            }
        }

        private void OnDrawGizmos()
        {
            if(BossPart==null || _pipe == null)
                return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.25f);
            Gizmos.DrawWireSphere(BossPart.Transform.position, 0.25f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, BossPart.Transform.position);
            Gizmos.DrawLine(transform.position, _pipe.transform.position);
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (_player != null && _actionMakeProgress == null)
            {
                _player.MakeFakeStopMove(true);
                _actionMakeProgress = StartCoroutine(MakeProgress());
            }
        }
    }
}