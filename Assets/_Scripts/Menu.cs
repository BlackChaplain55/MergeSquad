using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Image _blackScreen;
    [SerializeField] private AudioClip _bell;
    private void onEnable()
    {
        switch (GameController.Game.GameState)
        {
            case GameController.GameStates.Menu: _restartButton.interactable = false; break;
            case GameController.GameStates.Map: _restartButton.interactable = false;  break;
            case GameController.GameStates.Combat: _restartButton.interactable = true; break;
        }
    }

    public void StartGame()
    {
        StartScene(GameController.GameStates.Map, _bell);
    }

    public void StartScene(GameController.GameStates state, AudioClip? clip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (clip != null) audioSource.clip = clip;
        float delay = audioSource.clip.length;
        audioSource.Play();
        _blackScreen.gameObject.SetActive(true);
        _blackScreen.DOFade(1, delay);
        StartCoroutine(DelayedStart(delay, state));
    }

    private IEnumerator DelayedStart(float delay, GameController.GameStates state)
    {
        yield return new WaitForSeconds(delay);
        GameController.Game.LoadScene(state);
    }
}
