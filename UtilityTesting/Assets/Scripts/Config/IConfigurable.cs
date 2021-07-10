using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Config
{
    public interface IConfigurable
    {
        event System.Action<GameObject> OnConfigChanged;

        GameObject LoadConfig();
    }
}