using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : SourceWindow
{
    public RewardData Data;

    [SerializeField] TextMeshProUGUI _finalTimeDuration;
    [SerializeField] TextMeshProUGUI _resultKillCount;
    [SerializeField] Button _btnNext;
    [SerializeField] Button _btnMenu;

    public override SourceWindow Init(SourcePanel panel)
    {
        _btnNext.onClick.AddListener(Next);
        _btnMenu.onClick.AddListener(Menu);
        return base.Init(panel);
    }

    public override void OnOpen(params object[] data)
    {
        float seconds = Data.ElapsedTime;
        System.TimeSpan time = System.TimeSpan.FromSeconds(seconds);

        _finalTimeDuration.text = $"Time elapsed: {time.Minutes:D2}:{time.Seconds:D2}";

        base.OnOpen(data);

        StartCoroutineResult();
    }

    void StartCoroutineResult()
    {
        StopAllCoroutines();
        StartCoroutine(ResultCoroutine());
    }

    IEnumerator ResultCoroutine()
    {
        int targetKills = Data.KillCount;
        int currentKills = 0;

        float duration = 1.2f;
        float timer = 0f;

        _resultKillCount.text = $"Killed zombie:\n 0";

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            t = Mathf.SmoothStep(0, 1, t);

            currentKills = Mathf.RoundToInt(Mathf.Lerp(0, targetKills, t));
            _resultKillCount.text = $"Killed zombie:\n {currentKills}";

            yield return null;
        }

        _resultKillCount.text = $"Killed zombie:\n {targetKills}";
    }

    void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Menu()
    {
        SceneManager.LoadScene(1);
    }

    public override void Dispose()
    {
        _btnMenu.onClick.RemoveAllListeners();
        _btnNext.onClick.RemoveAllListeners();
    }
}
