using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jack.Utility.Events
{
    [Serializable] public class UnityIntEvent : UnityEvent<int> { };

    [Serializable] public class UnityFloatEvent : UnityEvent<float> { };

    [Serializable] public class UnityStringEvent : UnityEvent<string> { };

    [Serializable] public class UnityVector3Event : UnityEvent<Vector3> { };

    [Serializable] public class UnityColliderEvent : UnityEvent<Collider> { };

    [Serializable] public class UnityGameObjectEvent : UnityEvent<GameObject> { };

    [Serializable] public class UnityTransformEvent : UnityEvent<Transform> { };
}