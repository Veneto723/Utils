using System;
using System.Collections;
using GameLogic.Utils;
using UnityEngine;
using Utils.Scripts.Runtime.ObjectPool;

namespace Utils.Scripts.Runtime {
    public sealed class CommonTimer : MonoSingleton<CommonTimer> {
        /// <summary>
        /// 延迟<code>seconds</code>秒后执行<code>callback</code>。并在此后每<code>interval</code>秒执行1次，
        /// 直至达到循环上限次数<code>loopCount</code>。须注意的是<code>loopCount</code>指的是首次执行后，额外执行的次数。
        /// </summary>
        /// <param name="seconds">延迟秒数</param>
        /// <param name="callback">执行内容</param>
        /// <param name="loopCount">执行次数</param>
        /// <param name="interval">执行间隔</param>
        public void InvokeAction(float seconds, Action callback, int loopCount = 0, float interval = 1f) {
            StartCoroutine(TimerCoroutine(seconds, callback, loopCount, interval));
        }

        private static IEnumerator TimerCoroutine(float seconds, Action callback, int loopCount = 0,
            float interval = 1) {
            yield return new WaitForSeconds(seconds);
            callback.Invoke();

            var curr = 0;
            while (loopCount == -1 || curr < loopCount) {
                yield return new WaitForSeconds(interval);
                callback.Invoke();
                curr++;
            }
        }

        private static IEnumerator TimerCoroutine2(float countdown, Action frequentCall, Action callback,
            float frequentCountdown = 1, int loopCount = 0, float loopInterval = 1) {
            // TODO 期间频繁调用frequentCall, countdown结束时调用callback
            yield return new WaitForSeconds(frequentCountdown);
        }
    }
}