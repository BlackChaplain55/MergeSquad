using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
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
    public int Souls
    {
        get { return _souls; }
        private set
        {
            _souls = value;
            SoulsChanged(value);
        }
    }
    public int Crystals
    {
        get { return _crystals; }
        private set
        {
            _crystals = value;
            CrystalsChanged(value);
        }
    }
    public bool IsPaused { get { return _isPaused; } }
    public event Action<GameProgress> OnGameProgressChanged;
    public event Action<int> OnSceneChanged;
    public event Action<int> OnSoulsChanged;
    public event Action<int> OnCrystalsChanged;

    [SerializeField] private GameProgress gameProgress;
    private SoundController _sound;
    private int _souls;
    private int _crystals;
    private bool _isPaused;

    public void StartMusic(int sceneIndex) => _sound?.StartMusic(sceneIndex);
    public void StartMusic(AudioClip clip) => _sound?.StartMusic(clip);
    public void StopMusic() => _sound?.StopMusic();

    public void LoadScene(GameStates state) => SceneManager.LoadScene((int)state);
    public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

    public void LoadLevel(LevelSO data)
    {
        StartMusic(data.loadClip);
        LoadScene(GameStates.Combat);
    }

    public bool SpendSouls(int value)
    {
        int remainSouls = Souls - value;
        if (remainSouls < 0)
            return false;

        Souls = remainSouls;
        return true;
    }

    public void SetPause(bool flag)
    {
        _isPaused = flag;
        Time.timeScale = flag ? 0 : 1;
    }

    private void OnValidate()
    {
        if (_sound == null) _sound = GetComponent<SoundController>();
    }

    private void Awake()
    {
        if (Game == null)
        {
            DontDestroyOnLoad(gameObject);
            Game = this;
        }
        else if (Game != this)
        {
            Destroy(gameObject);
            return;
        }

        EverySceneChanged();

        GameProgress savedProgress = default;
        if (SaveSystem.Instance.TryLoadGame("saves", out savedProgress))
        {
            gameProgress = savedProgress;
            ArtifactsRepository?.Init(gameProgress.UnitArtifacts, gameProgress.ItemArtifacts);
        }
        
        EventBus.OnUnitDeath += unit => { if (unit.isEnemy) Souls += Settings.SoulsPerKill; };
        EventBus.OnBossDeath += () => { Crystals += Settings.CrystalsPerBossKill; };
        EventBus.OnFinalBossDeath += () => { Crystals += Settings.CrystalsPerFinalBossKill; };
        MergeData.InitResources();
    }

    private IEnumerator Start() {
        Application.targetFrameRate = 60;

        _sound?.StartMusic((int)Game.GameState);
        
        SceneManager.activeSceneChanged += ChangeSceneState;
        OnSceneChanged += _new => EverySceneChanged();
        OnSceneChanged += StartMusic;
    
        WaitForSeconds wait = new WaitForSeconds(15);
        while (true)
        {
            SaveSystem.Instance.TrySaveGame("saves", gameProgress);
            yield return wait;
        }
    }

    private void EverySceneChanged()
    {
        GameState = (GameStates)SceneManager.GetActiveScene().buildIndex;
        Souls = Settings.StartSouls;
    }

    private void ChangeSceneState(Scene oldScene, Scene newScene)
    {
        GameState = (GameStates)newScene.buildIndex;

        _sound?.StartMusic(newScene.buildIndex);
        OnSceneChanged?.Invoke(newScene.buildIndex);
    }

    private void SoulsChanged(int value) => OnSoulsChanged?.Invoke(value);
    private void CrystalsChanged(int value) => OnCrystalsChanged?.Invoke(value);

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ChangeSceneState;
        OnSceneChanged = null;
    }
}