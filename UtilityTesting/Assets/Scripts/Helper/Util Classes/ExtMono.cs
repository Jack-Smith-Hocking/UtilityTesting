using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Helper
{
    public class MonoInvoker 
    {
        private static MonoInvoker Instance = null;
        private MonoBehaviour m_mono;

        public MonoInvoker(MonoBehaviour mb) => m_mono = mb;

        public static MonoInvoker GetStaticInstance(MonoBehaviour mb)
        {
            if (Instance.IsNull()) Instance = new MonoInvoker(mb);

            Instance.m_mono = mb;

            return Instance;
        }

        #region InvokeAfter
        public void NextFrame(System.Action action) => After(action, null);
        public void EndOfFrame(System.Action action) => After(action, new WaitForEndOfFrame());

        public void FixedUpdate(System.Action action) => After(action, new WaitForFixedUpdate());

        public void After(System.Action action, float secondsDelay) => After(action, new WaitForSeconds(secondsDelay));
        public void AfterRandom(System.Action action, float minSeconds, float maxSeconds) => After(action, Random.Range(minSeconds, maxSeconds));

        public void After(System.Action action, YieldInstruction yieldInstruction = null) => m_mono.StartCoroutine(CoroutineAfter(action, yieldInstruction));

        private IEnumerator CoroutineAfter(System.Action action, YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;

            action?.Invoke();
        }
        #endregion

        #region InvokeRepeat
        /// <summary>
        /// Repeats an action each frame, forever
        /// </summary>
        /// <param name="action">Action to perform</param>
        public void FrameRepeat(System.Action action) => RepeatForever(action, null);
        /// <summary>
        /// Repeats an action with a delay, forever
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="secondsBetween">Time between performing the action</param>
        public void FrameRepeat(System.Action action, float secondsBetween) => RepeatForever(action, new WaitForSeconds(secondsBetween));

        /// <summary>
        /// Repeats an action a specified amount of times
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="repeatCount">Amount of times to perform, once per frame</param>
        public void FrameRepeat(System.Action<int> action, uint repeatCount) => RepeatCount(action, repeatCount, null);

        /// <summary>
        /// Repeat an action forever
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="yieldInstruction">The instructions for the yield (e.g. new WaitForSeonds(2))</param>
        public void RepeatForever(System.Action action, YieldInstruction yieldInstruction) => m_mono.StartCoroutine(CorutineRepeatForever(action, yieldInstruction));
        /// <summary>
        /// Repeats an action a specified amount of times
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="repeatCount">Amount of times to perform</param>
        /// <param name="yieldInstruction">The instructions for the yield (e.g. new WaitForSeonds(2))</param>
        public void RepeatCount(System.Action<int> action, uint repeatCount, YieldInstruction yieldInstruction) => m_mono.StartCoroutine(CoroutineRepeatCount(action, repeatCount, yieldInstruction));

        private IEnumerator CorutineRepeatForever(System.Action action, YieldInstruction yieldInstruction)
        {
            while (true)
            {
                yield return yieldInstruction;

                action?.Invoke();
            }
        }
        private IEnumerator CoroutineRepeatCount(System.Action<int> action, uint repeatCount, YieldInstruction yieldInstruction)
        {
            for (int _repeatIndex = 0; _repeatIndex < repeatCount; _repeatIndex++)
            {
                yield return yieldInstruction;

                action?.Invoke(_repeatIndex);
            }
        }
        #endregion

        #region InvokeWhen
        public void WaitFor<T>(System.Func<T> predicate, T desiredVal, System.Action tickAction = null, System.Action finishedAction = null, YieldInstruction yieldInstruction = null) where T : System.IComparable
        {
            m_mono.StartCoroutine(CoroutineWaitForValue(predicate, desiredVal, tickAction, finishedAction, yieldInstruction));
        }

        private IEnumerator CoroutineWaitForValue<K>(System.Func<K> predicate, K desiredVal, System.Action tickAction, System.Action finishedAction, YieldInstruction yieldInstruction) where K : System.IComparable
        {
            while (predicate().CompareTo(desiredVal) != 0)
            {
                yield return yieldInstruction;

                tickAction?.Invoke();
            }

            finishedAction?.Invoke();
        }
        #endregion
    }

    public static class ExtMono
    {
        public static MonoInvoker Invoker(this MonoBehaviour mb) => MonoInvoker.GetStaticInstance(mb);
    }
}