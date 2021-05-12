using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper.Utility
{
    public class MonoInvoker 
    {
        private static MonoInvoker m_instance = null;
        private MonoBehaviour m_mono;

        public MonoInvoker(MonoBehaviour mb) => m_mono = mb;

        public static MonoInvoker GetStaticInstance(MonoBehaviour mb)
        {
            if (m_instance.IsNull()) m_instance = new MonoInvoker(mb);

            m_instance.m_mono = mb;

            return m_instance;
        }

        #region InvokeAfter
        /// <summary>
        /// Invoke an action at the start of the next frame
        /// </summary>
        /// <param name="action">Action to perform</param>
        public void NextFrame(System.Action action) => After(action, null);
        /// <summary>
        /// Invoke an action at the end of this frame
        /// </summary>
        /// <param name="action">Action to perform</param>
        public void EndOfFrame(System.Action action) => After(action, new WaitForEndOfFrame());

        /// <summary>
        /// Wait for the next fixed update loop to invoke an action
        /// </summary>
        /// <param name="action">Action to perform</param>
        public void FixedUpdate(System.Action action) => After(action, new WaitForFixedUpdate());

        /// <summary>
        /// Invoke an action after some amount of time (game time, not realTime);
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="secondsDelay">Time before invoke</param>
        public void After(System.Action action, float secondsDelay) => After(action, new WaitForSeconds(secondsDelay));
        /// <summary>
        /// Invoke an action after some random amount of time, within parameters
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="minSeconds">Minimum seconds delay</param>
        /// <param name="maxSeconds">Maximum seconds delay</param>
        public void AfterRandom(System.Action action, float minSeconds, float maxSeconds) => After(action, Random.Range(minSeconds, maxSeconds));

        /// <summary>
        /// Invoke an action after some YieldInstruction, such as -> new WaitForSecondsRealTime(5)
        /// </summary>
        /// <param name="action">Action to perform</param>
        /// <param name="yieldInstruction">The yield instruction</param>
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
        /// <summary>
        /// Invoke an action when some predicate equals a given value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate to determine whether to invoke the finish action</param>
        /// <param name="desiredVal">The value that the predicate has to return to invoke finish action</param>
        /// <param name="tickAction">An action to perform each yield instruction -> WaitForEndOfFrame() would mean tick action performs at the end of each frame</param>
        /// <param name="finishedAction">The action to perform when the predicate evaluates to the desired value</param>
        /// <param name="yieldInstruction">The yield instruction that determines how often the predicate is tested, and how long before each tick action</param>
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