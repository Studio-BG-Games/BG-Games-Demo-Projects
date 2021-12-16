using System;
using System.Collections.Generic;
using Extension;
using Plugins.HabObject;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gameplay.Builds.Data
{
    [DisallowMultipleComponent]
    public class SizeOnMap : DataProperty
    {
        public Vector2Int Offset => _offset;
        public Vector2Int Size => _size;

        public HabObject Hab => _hab != null ? _hab : _hab = transform.parent.GetComponent<HabObject>();
        [SerializeField]private HabObject _hab;
        [SerializeField] private Vector2Int _offset;
        [SerializeField] private Vector2Int _size;

        private static AnimationCurve CurveX
        {
            get
            {
                if (_curveX != null)
                    return _curveX;
                var keys = new Keyframe[5];
                int i = 0;
                keys[i] = new Keyframe(0,0);
                i++;
                keys[i] = new Keyframe(0.25f,0);
                i++;
                keys[i] = new Keyframe(0.5f,-1);
                i++;
                keys[i] = new Keyframe(0.75f,-1);
                i++;
                keys[i] = new Keyframe(1,0);
                _curveX = new AnimationCurve(keys);
                _curveX.postWrapMode = _curveX.preWrapMode = WrapMode.Loop;
                return _curveX;
            }
        }
        private static AnimationCurve CurveZ
        {
            get
            {
                if (_curveZ != null)
                    return _curveZ;
                var keys = new Keyframe[5];
                int i = 0;
                keys[i] = new Keyframe(0,0);
                i++;
                keys[i] = new Keyframe(0.25f,-1);
                i++;
                keys[i] = new Keyframe(0.5f,-1);
                i++;
                keys[i] = new Keyframe(0.75f,0);
                i++;
                keys[i] = new Keyframe(1,0);
                _curveZ = new AnimationCurve(keys);
                _curveZ.postWrapMode = _curveZ.preWrapMode = WrapMode.Loop;
                return _curveZ;
            }
        }

        private static  AnimationCurve _curveX;
        private static AnimationCurve _curveZ;


        public static Vector3 GetModifacateXZ(float angelY) 
            => new Vector3(CurveX.Evaluate(angelY / 360f), 0, (CurveZ.Evaluate(angelY / 360f)));
        
        public static Vector3 SingModifacateXZ(float angelY)
        {
            var mod = GetModifacateXZ(angelY);
            return  new Vector3(Mathf.Sign(mod.x), 0, Mathf.Sign(mod.z));
        }

      
        public List<Vector3> GetAllEmployedCell()
        {
            var result = new List<Vector3>();
            
            MinPointAndMaxPoint(out var min, out var max);

            var mod = GetModifacateXZ(Hab.transform.eulerAngles.y);
            CustomFor(min.x, max.x, 1, x =>
            {
                CustomFor(min.z,max.z, 1, z =>
                {
                    var somePoint = new Vector3(x, 0, z)-mod;
                    result.Add(somePoint);
                });
            });
            return result;

        }

        private Vector3 CeilVector(Vector3 posTocheck) => 
            new Vector3(Mathf.Ceil(posTocheck.x), Mathf.Ceil(posTocheck.y), Mathf.Ceil(posTocheck.z));

        private void CustomFor(float x1, float x2, float step, Action<float> callback)
        {
            //x1 = Round2(x1);
            //x2 = Round2(x2);
            if(x1<x2)
                for (float i = x1; i < x2; i+=step)
                    callback?.Invoke(i);
            else
                for (float i = x2; i < x1; i += step)
                    callback?.Invoke(i);
        }

        
        private void MinPointAndMaxPoint(out Vector3 minPoint, out Vector3 maxPoint)
        {
            var posTocheck = Hab.transform.position;
            var prevVector = posTocheck;
            //posTocheck = CeilVector(posTocheck);
            Hab.transform.position = posTocheck;
            
            minPoint = Hab.transform.position + (Hab.transform.forward * _offset.y + Hab.transform.right * _offset.x);

            Vector2 sizeByAngel = SwapVector2Value(_size, Mathf.Abs(Mathf.Sin(Hab.transform.eulerAngles.y*Mathf.Deg2Rad)));
            maxPoint = minPoint
                       + _size.x * Hab.transform.right
                       + _size.y * Hab.transform.forward; 
                       

            maxPoint.x = Round(maxPoint.x);
            maxPoint.z = Round(maxPoint.z);
            minPoint.x = Round(minPoint.x);
            minPoint.z = Round(minPoint.z);
            
            Hab.transform.position = prevVector;
        }
        #if UNITY_EDITOR
        [ContextMenu("Debug cells")]
        private void Debgub_GetAllEmployedCell()
        {
            string debugMes = "";
            int i = 1;
            GetAllEmployedCell().ForEach(x=>
            {
                debugMes += $"[{i}: x:{x.x} y:{x.y} z:{x.z}]\n";
                i++;
            });
            
            Debug.Log(debugMes);
        }
        
        private void OnDrawGizmos()
        {
            if(Application.isPlaying)
                return;
            DrawCvadrat();
        }

        private void OnDrawGizmosSelected()
        {
            if(!Application.isPlaying)
                return;
            DrawCvadrat();
        }

        private void DrawCvadrat()
        {
            MinPointAndMaxPoint(out var minPoint, out var maxPoint);
            var sing = SingModifacateXZ(Hab.transform.eulerAngles.y);
            minPoint -= sing / 2;
            maxPoint -= sing / 2;
            Handles.Label(minPoint, $"offset, start here x:{minPoint.x} y:{minPoint.y} z:{minPoint.z}");
            Handles.Label(maxPoint, $"size, end here x:{maxPoint.x} y:{maxPoint.y} z:{maxPoint.z}");
            Gizmos.DrawLine(minPoint, maxPoint);
            Gizmos.color = Color.red;

            var p1 = new Vector3(minPoint.x, 0, maxPoint.z);
            var p2 = new Vector3(maxPoint.x, 0, minPoint.z);

            Gizmos.DrawLine(maxPoint, p1);
            Gizmos.DrawLine(maxPoint, p2);
            Gizmos.DrawLine(minPoint, p1);
            Gizmos.DrawLine(minPoint, p2);
        }
#endif

        private float Round(float x)
        {
            return (float) Math.Round(x, 1);
            
        }

        private float Round2(float x)
        {
            var ostatok = x % 0.5f;
            if(ostatok>0)
            {
                if (ostatok < 0.25f)
                    return x - ostatok;
                else
                    return x + 0.5f - ostatok;
            }
            else
            {
                if (ostatok > -0.25f)
                    return x - ostatok;
                else
                    return x + 0.5f - ostatok;
            }
        }

        public List<Vector3> GetAllEmployedCell(Vector3 position, Vector3 rotate)
        {
            var prevPos = Hab.transform.position;
            var prevRotate = Hab.transform.eulerAngles;
            
            Hab.transform.position = position;
            Hab.transform.eulerAngles = rotate;

            var result = GetAllEmployedCell();
            
            Hab.transform.position = prevPos;
            Hab.transform.eulerAngles = prevRotate;

            return result;
        }


        

        private Vector2 SwapVector2Value(Vector2 size, float normalProcent)
        {
            float allValue = size.x + size.y;
            float newX = (size.y * normalProcent) + (size.x * (1 - normalProcent));
            float newY = (size.x * normalProcent) + (size.y * (1 - normalProcent));
            return new Vector2(newX, newY);
        }

        private void OnValidate()
        {
            if (_size.x <= 0)
                _size.x =1;
            if (_size.y <= 0)
                _size.y = 1;
            if (_hab == null)
                _hab = transform.parent.GetComponent<HabObject>();
        }
    }
}