using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jack.Utility;

namespace Jack.Updater
{
    public enum UpdateCycle
    {
        NORMAL,
        FIXED,
        LATE
    }

    public static partial class FunctionUpdater
    {
        private static FunctionUpdateHandler HandlerInstance => s_singleton.Instance;
        private static MonoSingleton<FunctionUpdateHandler> s_singleton = new MonoSingleton<FunctionUpdateHandler>(nameof(FunctionUpdateHandler));

        /// <summary>
        /// Add an Action to the static update queue
        /// </summary>
        /// <param name="updateFunc">The function to update</param>
        /// <param name="functionName">The name of the function, used to cancel functions by name</param>
        /// <param name="active">Whether the function starts active</param>
        /// <param name="updateType">Which update cycle should be used (Normal, Fixed or Late)</param>
        /// <returns></returns>
        public static FunctionData CreateUpdater(Action updateFunc, string functionName = "", bool active = true, UpdateCycle updateType = UpdateCycle.NORMAL, int order = 0)
        {
            return CreateUpdater(() => { updateFunc.Invoke(); return true; }, functionName, active, updateType, order);
        }

        /// <summary>
        /// Add a Func to the static update queue
        /// </summary>
        /// <param name="updateFunc">The function to update, returns the stop condition</param>
        /// <param name="functionName">The name of the function, used to cancel functions by name</param>
        /// <param name="active">Whether the function starts active</param>
        /// <param name="updateType">Which update cycle should be used (Normal, Fixed or Late)</param>
        /// <returns></returns>
        public static FunctionData CreateUpdater(Func<bool> updateFunc, string functionName = "", bool active = true, UpdateCycle updateType = UpdateCycle.NORMAL, int order = 0)
        {
            FunctionData _updateData = new FunctionData(updateFunc, functionName, active, updateType, order);

            HandlerInstance.AddFunctionData(_updateData);

            return _updateData;
        }

        /// <summary>
        /// Remove all the listeners that have a specified name
        /// </summary>
        /// <param name="functionName">Name of the listeners to remove</param>
        public static void CancelAllWithName(string functionName) => HandlerInstance.RemoveAllByName(functionName);
    }
}