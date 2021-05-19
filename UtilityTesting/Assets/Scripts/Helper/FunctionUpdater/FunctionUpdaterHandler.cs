using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using Helper.Utility;

namespace Helper.Updater
{
    public class FunctionData
    {
        public UpdateCycle UpdateCycleType { get; private set; } = UpdateCycle.NORMAL;
        
        public string FunctionName { get; private set; } = "";

        public int SortOrder { get; private set; } = 0;

        public bool CancelUpdate { get; private set; } = false;
        public bool IsActive { get; private set; } = false;

        private Func<bool> m_updateFunction;

        public FunctionData(Func<bool> updateFunction, string functionName, bool isActive, UpdateCycle updateType, int sortOrder = 0)
        {
            m_updateFunction = updateFunction;

            IsActive = isActive;

            SortOrder = sortOrder;

            FunctionName = functionName;
            UpdateCycleType = updateType;
        }

        public bool Update()
        {
            if (!IsActive) return true;
            if (CancelUpdate) return false;

            bool _update = m_updateFunction.Invoke();
            CancelUpdate = !_update;

            return _update;
        }

        /// <summary>Stop the function from updating</summary>
        public void Pause() => IsActive = false;
        /// <summary>Resume updating the function</summary>
        public void Resume() => IsActive = true;
        /// <summary>Fully cancel this function, removes it from update cycle completely</summary>
        public void Cancel() => CancelUpdate = true;

        /// <summary>Set the update state of this function</summary>
        public void SetActive(bool active) => IsActive = active;
    }

    // Split the class into partial for organisation, holds the class definitions for
    // FunctionUpdateHandler and FunctionData
    public static partial class FunctionUpdater
    {
        private class FunctionUpdateHandler : MonoBehaviour
        {
            private Dictionary<UpdateCycle, List<FunctionData>> m_updateDict = new Dictionary<UpdateCycle, List<FunctionData>>();

            private List<FunctionData> m_cancelledData = new List<FunctionData>();

            /// <summary>
            /// Add a FunctionData to the update loop of this handler
            /// </summary>
            /// <param name="updateData"></param>
            public void AddFunctionData(FunctionData updateData)
            {
                if (updateData.IsNull()) return;

                if (m_updateDict.DoesNotContainKey(updateData.UpdateCycleType))
                {
                    m_updateDict[updateData.UpdateCycleType] = new List<FunctionData>(5);
                }

                m_updateDict[updateData.UpdateCycleType].Add(updateData);
                SortExecutionOrder(updateData.UpdateCycleType);
            }
            /// <summary>
            /// Remove a FunctionData from the update loop of this handler
            /// </summary>
            /// <param name="updateData"></param>
            public void RemoveUpdateData(FunctionData updateData)
            {
                if (updateData.IsNull()) return;
                if (m_updateDict.DoesNotContainKey(updateData.UpdateCycleType)) return;
                if (m_updateDict[updateData.UpdateCycleType].IsNull()) return;

                m_updateDict[updateData.UpdateCycleType].Remove(updateData);
            }

            /// <summary>
            /// Remove all listeners with the specified name from all update loops
            /// </summary>
            /// <param name="functionName"></param>
            public void RemoveAllByName(string functionName)
            {
                m_cancelledData.Clear();

                List<FunctionData> _updateList = m_updateDict.SelectMany(x => x.Value).ToList();
                m_cancelledData.AddRange(_updateList.Where(x => x.FunctionName == functionName));

                FlushCancelled();
            }

            private void UpdateData(UpdateCycle updateType)
            {
                if (!m_updateDict.ContainsKey(updateType)) return;
                m_cancelledData.Clear();

                for (int _dataIndex = 0; _dataIndex < m_updateDict[updateType].Count; _dataIndex++)
                {
                    FunctionData _data = m_updateDict[updateType][_dataIndex];

                    _data.Update();

                    if (!_data.CancelUpdate) continue;

                    m_cancelledData.Add(_data);
                }

                FlushCancelled();
            }
            private void FlushCancelled()
            {
                if (m_cancelledData.IsEmpty()) return;

                foreach (FunctionData _cancelledData in m_cancelledData)
                {
                    m_updateDict[_cancelledData.UpdateCycleType].Remove(_cancelledData);
                }

                m_cancelledData.Clear();
            }

            private void SortExecutionOrder(UpdateCycle sortCycle)
            {
                if (m_updateDict.DoesNotContainKey(sortCycle)) return;

                m_updateDict[sortCycle].Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
            }

            private void Awake() => this.hideFlags = HideFlags.HideInInspector;

            private void FixedUpdate() => UpdateData(UpdateCycle.FIXED);
            private void Update() => UpdateData(UpdateCycle.NORMAL);
            private void LateUpdate() => UpdateData(UpdateCycle.LATE);
        }
    }
}