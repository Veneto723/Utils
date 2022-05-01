using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Utils.Scripts.Runtime {
    public static class CommonUtils {
        #region UnityUtils

        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPos = default
            , int fontSize = 40, Color? color = null, TextAnchor anchor = TextAnchor.MiddleCenter,
            TextAlignment alignment = TextAlignment.Center, int sortingOrder = 0) {
            color ??= Color.white;
            var go = new GameObject("World_Text", typeof(TextMesh));
            var transform = go.transform;
            transform.SetParent(parent);
            transform.localPosition = localPos;
            var mesh = go.GetComponent<TextMesh>();
            mesh.text = text;
            mesh.fontSize = fontSize;
            mesh.anchor = anchor;
            mesh.color = (Color)color;
            mesh.alignment = alignment;
            mesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return mesh;
        }

        public static Vector3 GetMouseWorldPosition(Camera camera = null) {
            camera ??= Camera.main;
            System.Diagnostics.Debug.Assert(camera != null, nameof(camera) + " != null");
            var vec = camera.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0f;
            return vec;
        }

        #endregion

        #region TimeUtils

        /// <summary>
        /// 延迟<code>seconds</code>秒后执行<code>callback</code>。并在此后每<code>interval</code>秒执行1次，
        /// 直至达到循环上限次数<code>loopCount</code>。
        /// </summary>
        /// <param name="seconds">延迟秒数</param>
        /// <param name="callback">执行内容</param>
        /// <param name="loopCount">执行次数</param>
        /// <param name="interval">执行间隔</param>
        public static void InvokeAction(float seconds, Action callback, int loopCount = 0, float interval = 1f) {
            if (callback == null) {
                Debug.Log("InvokeAction: <Action callback> is null.");
                return;
            }

            if (CommonTimer.Instance is null) {
                Debug.Log("CommonTimer has not been initialized.");
                return;
            }

            CommonTimer.Instance.InvokeAction(seconds, callback, loopCount, interval);
        }

        #endregion

        #region BaseUtils

        /// <summary>
        /// 填充数组。
        /// </summary>
        /// <param name="arr">数组</param>
        /// <param name="item">填充物</param>
        /// <typeparam name="T">数组泛型</typeparam>
        public static void FillArray<T>(T[] arr, T item) {
            for (var i = 0; i < arr.Length; i++) {
                arr[i] = item;
            }
        }

        /// <summary>
        /// 填充集合。
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="item">填充物</param>
        /// <typeparam name="T">集合泛型</typeparam>
        public static void FillList<T>(List<T> list, T item) {
            for (var i = 0; i < list.Count; i++) {
                list[i] = item;
            }
        }

        public static void Shuffle<T>(ref List<T> list) {
            if (list == null || list.Count <= 0) return;
            for (var i = list.Count - 1; i > 0; i--) {
                SwapItem(list, Random.Range(0, i - 1), i);
            }
        }

        public static void SwapItem<T>(List<T> list, int i1, int i2) {
            Assert.IsTrue(i1 < list.Count && i1 >= 0 && i2 < list.Count && i2 >= 0);
            if (i1 == i2) return;
            (list[i1], list[i2]) = (list[i2], list[i1]);
        }

        #endregion

        #region MathUtils

        /// <summary>
        /// 保留<code>decimalPlace</code>位小数点。
        /// </summary>
        /// <param name="num">数</param>
        /// <param name="decimalPlace">小数点位数</param>
        public static float KeepDecimal(float num, int decimalPlace = 2) {
            return (float)Math.Round(num, decimalPlace);
        }

        public static float GetAngle(Vector3 selfDirection, Vector3 compareDirection) {
            var deg = Vector3.Angle(selfDirection, compareDirection);
            if (Vector2.Dot(selfDirection, compareDirection) < 0) deg = -deg;
            return deg;
        }

        public static void Validate(ref int num, int lowerBound = int.MinValue, int upperBound = int.MaxValue,
            bool loop = false) {
            if (lowerBound != int.MinValue && num < lowerBound) {
                num = loop ? upperBound : lowerBound;
            } else if (upperBound != int.MaxValue && num > upperBound) {
                num = loop ? lowerBound : upperBound;
            }
        }
        
        public static void Validate(ref float num, float lowerBound = float.MinValue, float upperBound = float.MaxValue,
            bool loop = false) {
            if (Math.Abs(lowerBound - float.MinValue) > 0.01f && num < lowerBound) {
                num = loop ? upperBound : lowerBound;
            } else if (Math.Abs(upperBound - float.MaxValue) > 0.01f && num > upperBound) {
                num = loop ? lowerBound : upperBound;
            }
        }

        public static bool IsValidate(IComparable num, IComparable lowerBound = null, IComparable upperBound = null) {
            return (lowerBound == null || num.CompareTo(lowerBound) >= 0) &&
                   (upperBound == null || num.CompareTo(upperBound) <= 0);
        }

        #endregion
    }
}