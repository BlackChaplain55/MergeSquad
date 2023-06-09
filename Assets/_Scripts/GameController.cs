using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SoundController))]

public class GameController : MonoBehaviour 
{
    [SerializeField] private PartyPanelController _partyPanel;

    public static GameController Game;
    public GameStates GameState;
    public GameSettings Settings;
    public PartyController Party;
    public MergeConroller Merge;

    private SoundController _sound;

    private void Awake()
    {
        GameState = GameStates.Menu;
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        switch (arg1.buildIndex)
        {
            case 0:
                GameState = GameStates.Menu;
                EventBus.onSceneChange?.Invoke(0);
                break;
            case 1:
                GameState = GameStates.Map;
                if (_partyPanel != null&&Game!=null) _partyPanel.Init();
                EventBus.onSceneChange?.Invoke(1);
                break;
            case 2:
                GameState = GameStates.Combat;
                EventBus.onSceneChange?.Invoke(2);
                break;
        }
        if (_sound != null) _sound.StartMusic();
    }

    private void Start() {
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
        if (Party == null) Party = GetComponent<PartyController>();
        if (_sound == null) _sound = GetComponent<SoundController>();
        _sound.StartMusic();
        if (_partyPanel!=null) _partyPanel.Init();
    }

    public void StopMusic()
    {
        _sound.StopMusic();
    }

    public void LoadScene(GameStates state)
    {
        _sound.StopMusic();
        if (state == GameStates.Map)
        {
            SceneManager.LoadScene(1);
        }
        else if (state == GameStates.Menu)
        {
            SceneManager.LoadScene(0);
        }
        else if (state == GameStates.Combat)
        {
            SceneManager.LoadScene(2);
        }     
    }

    public enum GameStates
    {
        Menu,
        Map,
        Combat,
        Unknown
    }
}