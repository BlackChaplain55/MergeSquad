using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]

public class Menu : MonoBehaviour
{
    [SerializeField] private Image _blackScreen;
    [SerializeField] private AudioClip _startGameAudio;

    public void StartGame()
    {
        StartScene(GameStates.Combat, _startGameAudio);
    }

    public void StartScene(GameStates state, AudioClip clip)
    {
        GameController.Game.StopMusic();
        AudioSource audioSource = GetComponent<AudioSource>();
        if (clip != null) audioSource.clip = clip;
        float delay = audioSource.clip.length;
        audioSource.Play();
        _blackScreen.gameObject.SetActive(true);
        _blackScreen.DOFade(1, delay);
        StartCoroutine(DelayedStart(delay, state));
    }

    private IEnumerator DelayedStart(float delay, GameStates state)
    {
        yield return new WaitForSeconds(delay);
        GameController.Game.LoadScene(state);
    }
}
