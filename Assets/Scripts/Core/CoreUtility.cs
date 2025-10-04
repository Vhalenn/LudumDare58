
#if UNITY_EDITOR
using UnityEditor;
using System.Diagnostics;
#endif

using UnityEngine;
using System;


public static class CoreUtility
{
    public static void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif

    }

    #region Enum

    public static string[] EnumNames(Type enumType)
    {
        return Enum.GetNames(enumType);

    }

    public static Array EnumValues(Type enumType)
    {
        return Enum.GetValues(enumType);
    }

    public static int EnumLength(Type enumType)
    {
        return EnumNames(enumType).Length;
    }

    public static object ParseEnumValue(Type type, string value)
    {
        return Enum.Parse(type, value); // String to Enum value
    }

    #endregion Enum

    public static Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, float angleY)
    {
        if (!angleIsGlobal) angleInDegrees += angleY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
                            0,
                            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // FOR TESTING - Gizmos && Others

    public static void DrawCircle(Color color, Vector3 pos, float radius)
    {
#if UNITY_EDITOR
        DrawCircle(color, pos, Vector3.up, radius);
#endif
    }
    public static void DrawCircle(Color color, Vector3 pos, Vector3 up, float radius)
    {
#if UNITY_EDITOR
        Handles.color = color;
        Handles.DrawWireDisc(pos, up, radius);
#endif
    }

    public static void DrawVisionCone(Vector3 pos, float angleY, float viewAngle, float radius, bool globalAngle)
    {
#if UNITY_EDITOR
        Handles.color = Color.yellow;
        float dirAngle = viewAngle * 0.5f + 90f - angleY;
        Vector3 lDirection = new(Mathf.Cos(Mathf.Deg2Rad * dirAngle), 0, Mathf.Sin(Mathf.Deg2Rad * dirAngle));

        Handles.DrawWireArc(pos, Vector3.up, lDirection, viewAngle, radius);
        Vector3 viewAngleA = DirFromAngle(-viewAngle * 0.5f, globalAngle, angleY);
        Vector3 viewAngleB = DirFromAngle(viewAngle * 0.5f, globalAngle, angleY);

        Handles.DrawLine(pos, pos + viewAngleA * radius);
        Handles.DrawLine(pos, pos + viewAngleB * radius);
#endif
    }

    public static void DrawText(Vector3 pos, string text, Color color, int size = 10)
    {
#if UNITY_EDITOR
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = size;
        headStyle.fontStyle = FontStyle.Bold;
        headStyle.normal.textColor = color;
        Handles.color = color;

        Handles.Label(pos, text, headStyle);
#endif
    }
}
