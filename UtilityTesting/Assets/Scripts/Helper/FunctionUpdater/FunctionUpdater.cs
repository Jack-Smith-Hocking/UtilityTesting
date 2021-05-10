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
        private static Singleton<FunctionUpdateHandler> m_singletonUpdateHandler = new Singleton<FunctionUpdateHandler>(nameof(FunctionUpdateHandler));
        private static FunctionUpdateHandler HandlerInstance => m_singletonUpdateHandler.Instance;

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

            HandlerInstance.AddUpdateData(_updateData);

            return _updateData;
        }

        public static void CancelAllWithName(string functionName) => HandlerInstance.RemoveAllByName(functionName);
    }
}