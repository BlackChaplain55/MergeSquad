using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public void SetPause(bool flag) => GameController.Game.SetPause(flag);
    public void LoadScene(int sceneId) => GameController.Game.LoadScene(sceneId);
}