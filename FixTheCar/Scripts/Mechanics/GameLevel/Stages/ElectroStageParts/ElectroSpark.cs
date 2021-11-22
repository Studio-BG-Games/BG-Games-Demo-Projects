using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathCreation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mechanics.GameLevel.Stages.ElectroStageParts
{
    public class ElectroSpark : MonoBehaviour
    {
        public event Action Finished;
        public event Action Fail;

        [Min(1)][SerializeField] private float _durationMovePath;
        [Min(1)][SerializeField] private float _durationFadeImage;
        [SerializeField] private Image _image;
        [SerializeField] private float _overlapRaduis; 
        
        private Coroutine _moveAction;
        private Tween _tweenImage;
        

        private void Awake() => _image.DOFade(0, 0.000001f);

        public void Move(PathCreator pathCreator)
        {
            if(_moveAction!=null)
                return;
            if(_tweenImage!=null)
                _tweenImage.Kill();
            _moveAction = StartCoroutine(MoveAction(pathCreator));
        }

        private IEnumerator MoveAction(PathCreator pathCreator)
        {
            float passDuration = 0;
            _image.DOFade(1, 0.0001f);
            while (passDuration<_durationMovePath)
            {
                Move(pathCreator, passDuration);
                if (CheckTapeAtBroke(RayCastTape()))
                    BrokeMove();
                passDuration += Time.deltaTime;
                yield return null;
            }
            passDuration = _durationMovePath;
            Move(pathCreator, passDuration);
            _moveAction = null;
            Finished?.Invoke();
        }

        private void BrokeMove()
        {
            if (_moveAction == null) 
                Debug.LogError("MoveAction is NULLL!!!");
            StopCoroutine(_moveAction);
            _moveAction = null;
            _tweenImage = _image.DOFade(0, _durationFadeImage).OnKill(()=>_tweenImage = null);
            Fail?.Invoke();
        }

        private bool CheckTapeAtBroke(List<TapePlace> rayCastTape)
        {
            if (rayCastTape.Count == 0)
                return false;
            foreach (var place in rayCastTape)
                if (!place.IsFixed)
                    return true;
            return false;
        }

        private void Move(PathCreator pathCreator, float passDuration) 
            => transform.position = pathCreator.path.GetPointAtTime(passDuration / _durationMovePath, EndOfPathInstruction.Stop);

        private List<TapePlace> RayCastTape()
        {
            var result = new List<TapePlace>();
            var resultsRay = Physics2D.OverlapCircleAll(transform.position, _overlapRaduis);
            foreach (var ray in resultsRay)
                if (ray.gameObject.TryGetComponent<TapePlace>(out var place))
                    result.Add(place);
            return result;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _overlapRaduis);
        }
    }
}