using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class handles about language
/// </summary>
public static class Languages
{
    /// <summary>
    /// Language to display currently
    /// </summary>
    public static Language languageDisplaying = Language.English;

    /// <summary>
    /// What language to handle
    /// </summary>
    public enum Language
    {
        English,
        Japanese
    }
}
