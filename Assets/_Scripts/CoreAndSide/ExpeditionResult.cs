using DG.Tweening;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionResult : MonoBehaviour
{
    [SerializeField] private GameObject expeditionStatistics;
    [SerializeField] private TextMeshProUGUI statisticsTitle;
    [SerializeField] private TextMeshProUGUI statisticsText;
    [SerializeField] private Button acceptResultButton;
    private Tween _titleTween;
    private Tween _textTween;

    private void Start()
    {
        EventBus.OnHeroDeath += Defeat;
        EventBus.OnBossDeath += Victory;
        acceptResultButton?.onClick.AddListener(LoadMeta);
    }

    private void LoadMeta()
    {
        GameController.Game.SetPause(false);
        expeditionStatistics.SetActive(false);
        GameController.Game.LoadScene(GameStates.Menu);
    }

    public void Defeat()
    {
        GameController.Game.SetPause(true);
        expeditionStatistics.SetActive(true);
        PrintStatistics("Поражение");
    }

    public void Victory()
    {
        GameController.Game.SetPause(true);
        expeditionStatistics.SetActive(true);
        PrintStatistics("Победа");
    }

    private void PrintStatistics(string title)
    {
        float typeSpeed = 40f;
        string statistics = GenerateStatistics();
        string titleContainer = string.Empty;
        string container = string.Empty;

        _titleTween?.Kill();
        _textTween?.Kill();
        statisticsTitle.SetText(string.Empty);
        statisticsText.SetText(string.Empty);

        float animationDuration = statistics.Length / typeSpeed;

        _titleTween = DOTween.To(() => titleContainer, v => titleContainer = v, title, title.Length / typeSpeed * 2.5f)
        .OnUpdate(() =>
        {
            statisticsTitle.SetText(titleContainer);
        })
        .SetUpdate(true);

        _titleTween.OnComplete(() =>
        {
            _textTween =
            DOTween.To(() => container, v => container = v, statistics, statistics.Length / typeSpeed)
            .OnUpdate(() =>
            {
                statisticsText.SetText(container);
            })
            .SetUpdate(true).SetEase(Ease.Linear);
        });
    }

    private string GenerateStatistics()
    {
        StringBuilder statistics = new();
        string[] headers =
        {
            "Время с начала экспедиции: ",
            "Пройдено метров: ",
            "Убито врагов: ",
            "Урона нанесено: ",
            "Собрано душ: ",
            "Скрафчено предметов: ",
            "Заклинаний произнесено: "
        };
        int[] values =
        {
            Random.Range(5, 360),
            Random.Range(20, 1000),
            Random.Range(0, 100),
            Random.Range(100, 40000),
            Random.Range(20, 2000),
            Random.Range(2, 100),
            0
        };

        for (int i = 0; i < headers.Length; i++)
        {
            statistics.Append(headers[i]);
            statistics.Append(values[i]);
            statistics.Append("\n");
        }

        return statistics.ToString();
    }

    private void OnDestroy()
    {
        EventBus.OnHeroDeath -= Defeat;
        EventBus.OnBossDeath -= Victory;
        acceptResultButton?.onClick.RemoveListener(LoadMeta);
    }
}
