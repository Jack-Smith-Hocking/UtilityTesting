using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Utility.Helper
{
    public class UniqueID : MonoBehaviour
    {
        private static Dictionary<string, UniqueID> m_allUniqueIDs = new Dictionary<string, UniqueID>();

        [SerializeField, ReadOnly] private string m_uniqueID = Guid.NewGuid().ToString();

        public string UniqueIdentifier => m_uniqueID;

        private void Awake()
        {
            SetUniqueID(this);
        }

        public static UniqueID GetUniqueID(string id)
        {
            if (m_allUniqueIDs.TryGetValue(id, out UniqueID _uniqueID))
            {
                return _uniqueID;
            }

            return null;
        }

        public static UniqueID GetUniqueID(UniqueID uniqueID)
        {
            if (uniqueID == null) return null;

            if (m_allUniqueIDs.TryGetValue(uniqueID.UniqueIdentifier, out UniqueID _uniqueID))
            {
                return _uniqueID;
            }

            return null;
        }

        private static void SetUniqueID(UniqueID uniqueID)
        {
            if (uniqueID == null) return;

            if (!m_allUniqueIDs.ContainsKey(uniqueID.UniqueIdentifier))
            {
                m_allUniqueIDs[uniqueID.UniqueIdentifier] = uniqueID;
            }
        }
    }
}