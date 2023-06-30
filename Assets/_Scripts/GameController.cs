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
    [field: SerializeField]public ArtifactsRepository ArtifactsRepository { get; private set; }
    public GameStates GameState { get; private set; }   
    public SlotSpawner SlotSpawner { get; private set; }
    [field: SerializeField] public GameSettings Settings { get; private set; }
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
    public event Action<int> OnSceneChanged;

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

        GameProgress savedProgress = default;
        if (SaveSystem.Instance.TryLoadGame("saves", out savedProgress))
        {
            gameProgress = savedProgress;
            ArtifactsRepository?.Init(gameProgress.UnitArtifacts, gameProgress.ItemArtifacts);
        }
            
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
            SaveSystem.Instance.TrySaveGame("saves", gameProgress);
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

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ChangeSceneState;
        OnSceneChanged = null;
    }
}