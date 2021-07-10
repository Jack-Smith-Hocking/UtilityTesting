using System.Collections;
using System.Collections.Generic;
using System.Text;
using Jack.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

public enum Comparison
{
    NONE,
    EQUAL,
    NOT_EQUAL,
    LESS_THAN,
    MORE_THAN
}

[System.Serializable, InlineProperty]
public struct LogPriorityDetails
{
    public bool m_suppressLogs;

    [Space]

    public Comparison m_priorityComparison;

    [HideIf(nameof(m_priorityComparison), Comparison.NONE)]
    public int m_logPriority;
}

[CreateAssetMenu(menuName = "ScriptableObject/DebugLogger", fileName = "New DebugLogger")]
public class DebugLogger : ScriptableObject
{
    public static DebugLogger ActiveLogger { get; private set; } = null;

    public static bool ActiveLogSuppressAllLogs => ActiveLogger.m_suppressAllLogs;

    public static LogPriorityDetails ActiveLogDetails => ActiveLogger.m_logDetails;
    public static LogPriorityDetails ActiveWarningDetails => ActiveLogger.m_warningDetails;
    public static LogPriorityDetails ActiveErrorDetails => ActiveLogger.m_errorDetails;

    [SerializeField] private bool m_isGlobalLogger = false;

    [Space]

    [SerializeField] private bool m_suppressAllLogs = false;

    [TabGroup("LogDetails", "Log"), HideLabel]
    [SerializeField] private LogPriorityDetails m_logDetails;
    [TabGroup("LogDetails", "Warning"), HideLabel]
    [SerializeField] private LogPriorityDetails m_warningDetails;
    [TabGroup("LogDetails", "Error"), HideLabel]
    [SerializeField] private LogPriorityDetails m_errorDetails;

    #region EditorValidation
    private void OnEnable()
    {
        if (ActiveLogger.IsNull())
        {
            UpdateActiveLogger(this);
        }
    }

    private void OnValidate()
    {
        bool _isActiveLogger = this == ActiveLogger;

        if (m_isGlobalLogger == true && _isActiveLogger == false)
        {
            UpdateActiveLogger(this);
        }
        if (m_isGlobalLogger == false && _isActiveLogger)
        {
            UpdateActiveLogger(this);
        }
    }

    private void UpdateActiveLogger(DebugLogger debugLogger)
    {
        if (ActiveLogger.IsNotNull())
        {
            ActiveLogger.m_isGlobalLogger = false;
        }

        ActiveLogger = debugLogger;
        ActiveLogger.m_isGlobalLogger = true;
    }
    #endregion

    #region Logging
    public static void Log(string message, int priority, UnityEngine.Object context = null)
    {
        if (IsLoggerInvalid() || IsPriorityValid(ActiveLogDetails, priority))
        {
            Debug.Log(FormatLog(message, priority), context);
        }
    }
    public static void LogWarning(string message, int priority, UnityEngine.Object context = null)
    {
        if (IsLoggerInvalid() || IsPriorityValid(ActiveWarningDetails, priority))
        {
            Debug.LogWarning(FormatLog(message, priority), context);
        }
    }
    public static void LogError(string message, int priority, UnityEngine.Object context = null)
    {
        if (IsLoggerInvalid() || IsPriorityValid(ActiveErrorDetails, priority))
        {
            Debug.LogError(FormatLog(message, priority), context);
        }
    }
    #endregion

    private static string FormatLog(string message, int priority) => $"[P:{priority}]".ToBold() + " " + message;

    #region LogValidation
    private static bool IsLoggerInvalid() => ActiveLogger == null;
    private static bool IsPriorityValid(LogPriorityDetails logPriorityDetails, int priority)
    {
        bool _suppressLog = logPriorityDetails.m_suppressLogs || ActiveLogSuppressAllLogs;

        if (_suppressLog) return false;

        int _logPriority = logPriorityDetails.m_logPriority;
        Comparison _comp = logPriorityDetails.m_priorityComparison;

        switch (_comp)
        {
            case Comparison.NONE: return true;
            case Comparison.EQUAL: return priority == _logPriority;
            case Comparison.NOT_EQUAL: return priority != _logPriority;
            case Comparison.LESS_THAN: return priority < _logPriority;
            case Comparison.MORE_THAN: return priority > _logPriority;

            default: return true;
        }
    }
    #endregion
}
