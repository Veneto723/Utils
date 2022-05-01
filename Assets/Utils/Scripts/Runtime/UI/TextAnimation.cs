using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Scripts.Runtime.ObjectPool;

namespace Utils.Scripts.Runtime.UI {
    public class TextAnimation : MonoSingleton<TextAnimation> {
        private static IEnumerator textFadeOut_StaticIEnumerator(Text textObject, float time) {
            Color textColor = textObject.color;
            textObject.color = new Color(textColor.r, textColor.g, textColor.b, 1.0f);
            while (time != 0.0f && textObject.color.a > 0.0f) {
                textObject.color = new Color(textColor.r, textColor.g, textColor.b,
                    textObject.color.a - (Time.deltaTime / time));
                yield return null;
            }

            textObject.color = new Color(textColor.r, textColor.g, textColor.b, 0.0f);
        }


        private static IEnumerator textFadeIn_StaticIEnumerator(Text textObject, float time) {
            Color textColor = textObject.color;
            textObject.color = new Color(textColor.r, textColor.g, textColor.b, 0.0f);
            while (time != 0.0f && textObject.color.a < 1.0f) {
                textObject.color = new Color(textColor.r, textColor.g, textColor.b,
                    textObject.color.a + (Time.deltaTime / time));
                yield return null;
            }

            textObject.color = new Color(textColor.r, textColor.g, textColor.b, 1.0f);
        }

        public void FadeOut(Text textObject, float time = 1) {
            StartCoroutine(textFadeOut_StaticIEnumerator(textObject, time));
        }


        public void FadeIn(Text textObject, float time = 1) {
            StartCoroutine(textFadeIn_StaticIEnumerator(textObject, time));
        }
    }
}