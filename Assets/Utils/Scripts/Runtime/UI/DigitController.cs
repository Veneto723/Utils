using System;
using UnityEngine;
using Utils.Scripts.Runtime.ObjectPool;

namespace Utils.Scripts.Runtime.UI {
    public class DigitController : MonoBehaviour {
        public GameObject digitPrefab;
        public Sprite[] digitSprites;
        
        public int value;
        public int digitNum;
        public float fontSize = 14;
        public int orderInLayer;
        public bool alignLeft = true;
        private float ActualFontSize => fontSize * 0.01f;
        private float Spacing => ActualFontSize * 2f;

        private void Start() {
            Init(value, fontSize);
        }

        public void Init(int val, float size) {
            if (value < 0 || size < 0) return;
            value = val;
            fontSize = size;
            if (digitSprites == null || digitSprites.Length == 0) return; // 缺少字体素材

            var digitStr = val.ToString();
            digitNum = digitStr.Length;
            for (var i = 0; i < digitNum; i++) {
                var go = PoolManager.SpawnObject(digitPrefab);
                go.transform.localScale = new Vector3(ActualFontSize, ActualFontSize);
                go.transform.position = transform.position;
                go.transform.position += new Vector3((alignLeft ? i : i - digitNum + 1) * Spacing, 0);
                var component = go.GetComponent<SpriteRenderer>();
                component.sprite = digitSprites[int.Parse(digitStr[i].ToString())];
                component.sortingOrder = orderInLayer;
                go.transform.SetParent(gameObject.transform);
            }
        }

        public void SetValue(int val) {
            if (digitSprites == null || digitSprites.Length == 0) return; // 缺少字体素材
            var digitStr = val.ToString();
            if (digitNum < digitStr.Length) {
                for (var _ = 0; _ < digitStr.Length - digitNum; _++) {
                    var go = PoolManager.SpawnObject(digitPrefab);
                    go.transform.SetParent(transform);
                }
            } else if (digitNum > digitStr.Length) {
                for (var _ = 0; _ < digitNum - digitStr.Length; _++) {
                    PoolManager.ReleaseObject(GetComponentInChildren<SpriteRenderer>().gameObject);
                }
            }

            var renderers = GetComponentsInChildren<SpriteRenderer>();
            for (var i = 0; i < digitStr.Length; i++) {
                renderers[i].sprite = digitSprites[int.Parse(digitStr[i].ToString())];
                renderers[i].transform.localScale = new Vector3(ActualFontSize, ActualFontSize);
                renderers[i].transform.position = transform.position;
                renderers[i].transform.position += new Vector3((alignLeft ? i : i -  digitStr.Length + 1) * Spacing, 0);
                
            }
        }

    }
}