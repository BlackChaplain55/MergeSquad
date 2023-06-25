using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SoundController))]

public class GameController : MonoBehaviour 
{
    public static GameController Game { get; private set; }
    public event Action<int> OnSceneChanged;
    public GameStates GameState;
    public GameSettings Settings;
    public SlotSpawner SlotSpawner;
    public GameProgress GameProgress
    {
        get { return gameProgress; }
        set
        {
            gameProgress = value;
            OnGameProgressChanged?.Invoke(this.GameProgress);
        }
    }
    public event Action<GameProgress> OnGameProgressChanged;

    [SerializeField] private GameProgress gameProgress;
    private SoundController _sound;

    public void StartMusic(int sceneIndex) => _sound?.StartMusic(sceneIndex);
    public void StartMusic(AudioClip clip) => _sound?.StartMusic(clip);
    public void StopMusic() => _sound?.StopMusic();

    public void LoadScene(GameStates state) => SceneManager.LoadScene((int)state);

    private void OnValidate()
    {
        if (_sound == null) _sound = GetComponent<SoundController>();
    }

    private void Awake()
    {
        GameState = (GameStates)SceneManager.GetActiveScene().buildIndex;
        if (Settings == null)
            Settings = GetComponent<GameSettings>();

        TryLoadGame("saves");
    }

    private IEnumerator Start() {
        Application.targetFrameRate = 60;
        if (Game == null)
        {
            DontDestroyOnLoad(gameObject);
            Game = this;
        }
        else if (Game != this)
        {
            Destroy(gameObject);
        }

        _sound?.StartMusic((int)Game.GameState);
        
        SceneManager.activeSceneChanged += ChangeSceneState;
        OnSceneChanged += _new => Awake();
        OnSceneChanged += StartMusic;
    
        WaitForSeconds wait = new WaitForSeconds(15);
        while (true)
        {
            TrySaveGame("saves");
            yield return wait;
        }
    }

    private void ChangeSceneState(Scene oldScene, Scene newScene)
    {
        GameState = (GameStates)newScene.buildIndex;

        _sound?.StartMusic(newScene.buildIndex);
        OnSceneChanged?.Invoke(newScene.buildIndex);
    }

    public void LoadLevel(LevelSO data)
    {
        StartMusic(data.loadClip);
        LoadScene(GameStates.Combat);
    }

    private bool TryLoadGame(string saveName)
    {
        //Debug.Log("Persistent data path : " + Application.persistentDataPath);
        string savePath = $"{Application.persistentDataPath}/{saveName}.json";
        if (File.Exists(savePath))
        {
            string saveData = File.ReadAllText(savePath);
            try
            {
                //GameProgress = JsonConvert.DeserializeObject<GameProgress>(saveData);
                GameProgress = JsonUtility.FromJson<GameProgress>(saveData);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Saved data in file cannot be converted to GameProgress : " + e);
            }
            return false;
        }
        else
            { return false; }
    }

    private bool TrySaveGame(string saveName)
    {
        string savePath = $"{Application.persistentDataPath}/{saveName}.json";
        using (var fileStream = new FileStream(savePath, FileMode.OpenOrCreate))
        {
            //byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_gameProgress));
            byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(gameProgress));
            fileStream.Write(data);
        }
        return true;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ChangeSceneState;
        OnSceneChanged = null;
    }
}

[Serializable]
public struct GameProgress : INotifyPropertyChanged
{
    public int UnlockedLevel;
    public int CurrentLevel;
    public event PropertyChangedEventHandler PropertyChanged;

    public GameProgress(int unlockedLevel = 1, int currentLevel = 1)
    {
        UnlockedLevel = unlockedLevel;
        CurrentLevel = currentLevel;
        PropertyChanged = null;
    }

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}