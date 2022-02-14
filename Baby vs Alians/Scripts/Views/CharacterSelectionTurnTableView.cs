using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class CharacterSelectionTurnTableView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _turnTable;

        private List<Quaternion> _targetRotations = new List<Quaternion>();
        private bool _isRotating;
        private float _rotationProgress;

        private float _rotationSpeed = 3;

        #endregion


        #region Properties

        public Transform TurnTable => _turnTable;

        #endregion


        #region UnityMethods

        private void Update()
        {
            if (_targetRotations.Count <= 0)
                return;

            if (!_isRotating)
            {
                _isRotating = true;
                _rotationProgress = 0;
            }

            if (_isRotating)
            {
                if (_rotationProgress < 1)
                {
                    _rotationProgress += Time.deltaTime * _rotationSpeed;
                    _turnTable.localRotation = Quaternion.Lerp(_turnTable.localRotation, _targetRotations[0], _rotationProgress);
                }
                else
                {
                    _targetRotations.RemoveAt(0);
                    _isRotating = false;
                }
            }
        }

        #endregion


        #region Methods

        public void RotateByAngle(float angle)
        {
            var rotation = _targetRotations.Count > 0 ? _targetRotations[_targetRotations.Count-1].eulerAngles : _turnTable.localEulerAngles;
            rotation.y += angle;

            _targetRotations.Add(Quaternion.Euler(rotation));
        }

        #endregion
    }
}