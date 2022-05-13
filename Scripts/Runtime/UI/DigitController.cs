using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Scripts.Runtime.UI {
    public class DigitController : MonoBehaviour {
        public enum AlignMode {
            Left,
            Center,
            Right
        }

        public Sprite[] digitSprites;

        public long value;
        public float fontSize = 14;
        public float spacing = 0.5f;
        public int orderInLayer;
        public AlignMode alignMode = AlignMode.Left;
        public bool displayOnAwake = true;
        public bool debugMode;

        [HideInInspector]
        public int digitNum;

        private float ActualFontSize => fontSize * 0.05f;

        [HideInInspector]
        public List<GameObject> objectsPool = new List<GameObject>();

        private void OnValidate() {
            if (value < 0) value = 0;
            if (fontSize < 0) fontSize = 0;
        }
        
        public void SetValue(int val) {
            if (val < 0) return;
            value = val;
            UpdateUI();
        }

        private void Awake() {
            if (debugMode) print($"Load {digitSprites.Length:D} digit sprite(s).");
            if (displayOnAwake) UpdateUI();
        }

        private void UpdateUI() {
            // sanity check
            objectsPool.RemoveAll(obj => obj == null);
            if (digitSprites.Length < 10) {
                if (debugMode) print($"Missing digit sprite. Only have {digitSprites.Length} sprite(s)");
                return;
            }

            // Instantiate game objects
            digitNum = value.ToString().Length;
            if (digitNum != objectsPool.Count) {
                var diff = digitNum - objectsPool.Count;
                for (var i = 0; i < Math.Abs(diff); i++) {
                    if (diff > 0) {
                        var go = new GameObject("Digit");
                        go.AddComponent<SpriteRenderer>();
                        go.transform.SetParent(transform);
                        objectsPool.Add(go);
                    } else {
                        DestroyImmediate(objectsPool.Last());
                        objectsPool.Remove(objectsPool.Last());
                    }
                }
            }

            // update digits UI
            for (var i = 0; i < objectsPool.Count; i++) {
                var component = objectsPool[i].GetComponent<SpriteRenderer>();
                component.sprite = digitSprites[int.Parse(value.ToString()[i].ToString())];
                component.sortingOrder = orderInLayer;
                component.transform.localScale = new Vector3(ActualFontSize, ActualFontSize);
                component.transform.localPosition = alignMode switch {
                    AlignMode.Left => new Vector3(i * spacing, 0),
                    AlignMode.Right => new Vector3((i - objectsPool.Count + 1) * spacing, 0),
                    AlignMode.Center => new Vector3((i - objectsPool.Count / 2) * spacing, 0),
                    _ => new Vector3((i - objectsPool.Count / 2) * spacing, 0)
                };
            }
        }

        [CustomEditor(typeof(DigitController))]
        [CanEditMultipleObjects]
        public class DigitControllerEditor : Editor {
            public override void OnInspectorGUI() {
                base.OnInspectorGUI();
                if (GUILayout.Button("Display")) {
                    ((DigitController)target).UpdateUI();
                }
            }
        }
    }
}