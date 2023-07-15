using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool IsPaused { get { return _isPaused; } }
    public event Action<GameProgress> OnGameProgressChanged;
    public event Action<int> OnSceneChanged;
    public event Action<int> OnSoulsChanged;

    [SerializeField] private GameProgress gameProgress;
    private SoundController _sound;
    private int _souls;
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

    public bool TrySpendSouls(int value)
    {
        int remainSouls = Souls - value;
        if (remainSouls < 0)
            return false;

        Souls = remainSouls;
        return true;
    }

    public bool TrySpendCrystals(int value)
    {
        int remainCrystals = GameProgress.Crystals - value;
        if (remainCrystals < 0)
            return false;

        GameProgress.Crystals = remainCrystals;
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

        GameProgress savedProgress = new(ArtifactsRepository, ArtifactsRepository.UnitArtifactsData, ArtifactsRepository.ItemArtifactsData, Settings.StartCrystals);
        
        if (SaveSystem.Instance.TryLoadGame("saves", out savedProgress))
            ArtifactsRepository?.Init(gameProgress.UnitArtifacts, gameProgress.ItemArtifacts);

        gameProgress = savedProgress;
        
        EventBus.OnUnitDeath += unit => { if (unit.isEnemy) Souls += Settings.SoulsPerKill; };
        EventBus.OnBossDeath += () => { GameProgress.Crystals += Settings.CrystalsPerBossKill; };
        EventBus.OnFinalBossDeath += () => { GameProgress.Crystals += Settings.CrystalsPerFinalBossKill; };
        MergeData.InitResources();
    }

    private IEnumerator Start() {
        Application.targetFrameRate = 60;

        _sound?.StartMusic((int)Game.GameState);
        
        SceneManager.activeSceneChanged += ChangeSceneState;
        OnSceneChanged += _new => EverySceneChanged();
        OnSceneChanged += StartMusic;
    
        WaitForSeconds wait = new WaitForSeconds(3);
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

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ChangeSceneState;
        OnSceneChanged = null;
    }
}