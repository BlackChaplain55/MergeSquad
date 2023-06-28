using UnityEditor;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
