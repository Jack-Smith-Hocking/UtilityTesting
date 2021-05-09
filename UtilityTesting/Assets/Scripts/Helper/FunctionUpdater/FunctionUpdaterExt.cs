using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Utility.Helper
{
    // Split the class into partial for organisation, holds the class definitions for
    // FunctionUpdateHandler and FunctionData
    public static partial class FunctionUpdater
    {
        private class FunctionUpdateHandler : MonoBehaviour
        {
            private Dictionary<UpdateType, List<FunctionData>> m_updateDict = new Dictionary<UpdateType, List<FunctionData>>();

            private List<FunctionData> m_cancelledData = new List<FunctionData>();

            public void AddUpdateData(FunctionData updateData)
            {
                if (updateData == null) return;
                if (!m_updateDict.ContainsKey(updateData.UpdateStamp))
                {
                    m_updateDict[updateData.UpdateStamp] = new List<FunctionData>(10);
                }

                m_updateDict[updateData.UpdateStamp].Add(updateData);
            }
            public void RemoveUpdateData(FunctionData updateData)
            {
                if (updateData == null) return;
                if (!m_updateDict.ContainsKey(updateData.UpdateStamp)) return;
                if (m_updateDict[updateData.UpdateStamp] == null) return;

                m_updateDict[updateData.UpdateStamp].Remove(updateData);
            }

            public void RemoveAllByName(string functionName)
            {
                m_cancelledData.Clear();

                List<FunctionData> _updateList = m_updateDict.SelectMany(x => x.Value).ToList();
                m_cancelledData.AddRange(_updateList.Where(x => x.FunctionName == functionName));

                FlushCancelled();
            }

            private void UpdateData(UpdateType updateType)
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
                if (m_cancelledData.Count == 0) return;

                foreach (FunctionData _cancelledData in m_cancelledData)
                {
                    m_updateDict[_cancelledData.UpdateStamp].Remove(_cancelledData);
                }

                m_cancelledData.Clear();
            }

            private void Awake() => this.hideFlags = HideFlags.HideInInspector;

            private void FixedUpdate() => UpdateData(UpdateType.FIXED);
            private void Update() => UpdateData(UpdateType.NORMAL);
            private void LateUpdate() => UpdateData(UpdateType.LATE);
        }

        public class FunctionData
        {
            public UpdateType UpdateStamp { get; private set; } = UpdateType.NORMAL;
            public string FunctionName { get; private set; } = "";
            public bool CancelUpdate { get; private set; } = false;
            public bool IsActive { get; private set; } = false;

            private Func<bool> m_updateFunction;

            public FunctionData(Func<bool> updateFunction, string functionName, bool isActive, UpdateType updateType)
            {
                m_updateFunction = updateFunction;
                IsActive = isActive;

                FunctionName = functionName;
                UpdateStamp = updateType;
            }

            public bool Update()
            {
                if (!IsActive) return true;
                if (CancelUpdate) return false;

                bool _update = m_updateFunction.Invoke();
                CancelUpdate = !_update;

                return _update;
            }

            public void Pause() => IsActive = false;
            public void Resume() => IsActive = true;
            public void Cancel() => CancelUpdate = true;

            public void SetActive(bool active) => IsActive = active;
        }
    }
}