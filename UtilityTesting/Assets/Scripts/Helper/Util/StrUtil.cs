using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jack.Utility
{
    public static class StrUtil
    {
        public static string SetColour(string text, Color colour) => text.SetColour(colour);

        public static string ExcludeMultiple(string text, params string[] wordsToExlude)
        {
            foreach (var _word in wordsToExlude)
            {
                text = text.Exclude(_word);
            }

            return text;
        }
    }

    public static class Str_Ext
    {
        public static string Exclude(this string text, string wordToExclude) => text.Replace(wordToExclude, "");
        public static string Exclude(string text, params string[] wordsToExlude)
        {
            foreach (var _word in wordsToExlude) text = text.Exclude(_word);

            return text;
        }

        public static string ToBold(this string text) => "<b>" + text + "</b>";
        public static string ToItalic(this string text) => "<i>" + text + "</i>";
        public static string ToStrike(this string text) => "<s>" + text + "</s>";

        public static string NewLine(this string text) => text + "\n";

        public static string SetSize(this string text, int size) => $"<size={size}>" + text + "</size>";
        public static string SetColour(this string text, Color color)
        {
            string _hexCol = "#" + ColorUtility.ToHtmlStringRGBA(color);
            return $"<color={_hexCol}>" + text + "</color>";
        }
    }
}