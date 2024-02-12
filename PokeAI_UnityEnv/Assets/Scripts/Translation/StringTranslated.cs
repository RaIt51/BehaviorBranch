using UnityEngine;

/// <summary>
/// String data that has data for each language
/// </summary>
[CreateAssetMenu(fileName = "Translation", menuName = "ScriptableObjects/Translation", order = 1)]
public class StringTranslated : ScriptableObject
{
    public string english;
    public string japanese;

    /// <summary>
    /// Get the corresponding string data for the language
    /// </summary>
    /// <param name="language">What language</param>
    /// <returns>String wanted</returns>
    public string Get(Languages.Language language)
    {
        switch (language)
        {
            case Languages.Language.English:
                return english;
            case Languages.Language.Japanese:
                return japanese;
            default:
                return english;
        }
    }

    /// <summary>
    /// Set language data by code
    /// </summary>
    public void Set(string english, string japanese)
    {
        this.english = english;
        this.japanese = japanese;
    }
}