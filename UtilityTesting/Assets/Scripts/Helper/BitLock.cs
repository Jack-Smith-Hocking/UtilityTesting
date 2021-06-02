using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jack.Utility
{
    public class BitLock
    {
        public bool IsLocked { get { return m_lockVal != 0; } }

        private int m_lockVal = 0;

        /// <summary>
        /// Set m_lockVal back to 0
        /// </summary>
        public void Reset() => m_lockVal = 0;

        /// <summary>
        /// Set the 'nth' bit
        /// </summary>
        /// <param name="nth"></param>
        public void LockBit(int nth) => m_lockVal = m_lockVal | (1 << nth);

        /// <summary>
        /// Unlock the 'nth' bit
        /// </summary>
        /// <param name="nth"></param>
        public void UnlockBit(int nth) => m_lockVal = m_lockVal & ~(1 << nth);

        /// <summary>
        /// Check whether the 'nth' bit is set
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public bool IsBitSet(int bit) => (m_lockVal & (1 << bit)) != 0;
    }
}