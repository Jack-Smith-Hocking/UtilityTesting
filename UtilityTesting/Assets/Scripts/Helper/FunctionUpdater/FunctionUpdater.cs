using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Helper
{
    public enum UpdateType
    {
        NORMAL,
        FIXED,
        LATE
    }

    public static partial class FunctionUpdater
    {
        private static GameObject m_updateHandlerObject;
        private static FunctionUpdateHandler m_updateHandler;

        /// <summary>
        /// Check if there is an active FunctionUpdateHandler, if there isn't, create one
        /// </summary>
        private static void InitialiseIfRequired()
        {
            if (m_updateHandlerObject.IsNull())
            {
                m_updateHandlerObject = new GameObject("FunctionUpdater_Handler");
                m_updateHandler = m_updateHandlerObject.AddComponent(typeof(FunctionUpdateHandler)) as FunctionUpdateHandler;
            }
        }

        /// <summary>
        /// Add an Action to the static update queue
        /// </summary>
        /// <param name="updateFunc">The function to update</param>
        /// <param name="functionName">The name of the function, used to cancel functions by name</param>
        /// <param name="active">Whether the function starts active</param>
        /// <param name="updateType">Which update cycle should be used (Normal, Fixed or Late)</param>
        /// <returns></returns>
        public static FunctionData CreateUpdater(Action updateFunc, string functionName = "", bool active = true, UpdateType updateType = UpdateType.NORMAL)
        {
            return CreateUpdater(() => { updateFunc.Invoke(); return true; }, functionName, active, updateType);
        }

        /// <summary>
        /// Add a Func to the static update queue
        /// </summary>
        /// <param name="updateFunc">The function to update, returns the stop condition</param>
        /// <param name="functionName">The name of the function, used to cancel functions by name</param>
        /// <param name="active">Whether the function starts active</param>
        /// <param name="updateType">Which update cycle should be used (Normal, Fixed or Late)</param>
        /// <returns></returns>
        public static FunctionData CreateUpdater(Func<bool> updateFunc, string functionName = "", bool active = true, UpdateType updateType = UpdateType.NORMAL)
        {
            FunctionData _updateData = new FunctionData(updateFunc, functionName, active, updateType);

            InitialiseIfRequired();

            m_updateHandler.AddUpdateData(_updateData);

            return _updateData;
        }

        public static void CancelAllWithName(string functionName) => m_updateHandler.RemoveAllByName(functionName);
    }
}