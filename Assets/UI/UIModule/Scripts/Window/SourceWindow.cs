using DG.Tweening;
using UnityEngine;

public abstract class SourceWindow : MonoBehaviour
{
    protected SourcePanel _panel;

    [Header("Animation Settings")]
    public float openDuration = 0.4f;
    public float closeDuration = 0.25f;
    public float punchScale = 0.2f; // сила "прыжка"

    private Tween _currentTween;
    public virtual SourceWindow Init(SourcePanel panel)
    {
        _panel = panel;
        gameObject.SetActive(false);
        return this;
    }

    public virtual void OnInject()
    {

    }

    public virtual void OnOpen(params object[] data)
    {
        gameObject.SetActive(true);

        // сбросим текущие твины, если панель быстро переоткрываетс€
        _currentTween?.Kill();

        // начальное состо€ние
        transform.localScale = Vector3.one * 0.8f;

        // плавное увеличение и "прыжок"
        _currentTween = transform.DOScale(1f, openDuration)
            .SetEase(Ease.OutBack) // м€гкий вылет
            .SetUpdate(true); // не зависит от Time.timeScale (если нужно в паузе)
    }

    public virtual void OnClose()
    {
        _currentTween?.Kill();

        // эффект "ухода" с лЄгким сжатием
        _currentTween = transform
            .DOScale(0.8f, closeDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                transform.localScale = Vector3.one; // сбросим масштаб
            })
            .SetUpdate(true);
    }

    public abstract void Dispose();

}
