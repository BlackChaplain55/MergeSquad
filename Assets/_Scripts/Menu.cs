using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Image _blackScreen;
    [SerializeField] private AudioClip _bell;
    private void OnEnable()
    {
        if (GameController.Game == null) return;

        bool isInCombatState = GameController.Game.GameState == GameStates.Combat;

        _restartButton.interactable = isInCombatState;
    }

    public void StartGame()
    {
        StartScene(GameStates.Map, _bell);
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
