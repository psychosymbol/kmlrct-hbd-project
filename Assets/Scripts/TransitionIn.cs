using DG.Tweening;
using UnityEngine;
using static MainMenuController;

public class TransitionIn : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField]
    private RectTransform transitionBase;
    [SerializeField]
    private RectTransform transitionBG;

    public TinyTimTimTheTutorialMaster TinyTimTimTheTutorialMaster;

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvasGroup.alpha = 1f;
        TransitionToCurrentScene();
    }

    void TransitionToCurrentScene()
    {
        RectTransform currentTrans = transitionBase;
        RectTransform currentTransBG = transitionBG;
        currentTransBG.DOScale(new Vector3(1.05f, 1.1f, 1), 1).From(new Vector3(15, 15, 1)).SetDelay(0).OnComplete(() =>
        {
            currentTrans.DOScale(new Vector3(0, 0, 1), 1).From(new Vector3(1, 1, 1)).SetDelay(.25f).OnComplete(() =>
            {
                TinyTimTimTheTutorialMaster.StartZoomTutorial();
            });
        });

    }
}
