using DG.Tweening;
using UnityEngine;


public class PageController : MonoBehaviour
{
    public RectTransform pageRT;
    public CanvasGroup pageCG;
    public CanvasGroup numCG;
    public CanvasGroup numFlipCG;

    public enum FlipSide
    {
        Left,
        Right,
    }

    public void FlipPage(float time, float delay, int siblingIndex, FlipSide side = FlipSide.Left)
    {
        switch (side)
        {
            default:
            case FlipSide.Left:

                pageRT.DORotate(new Vector3(0, 180, 0), time)
                    .SetDelay(delay)
                    .From(new Vector3(0, 0, 0))
                    .OnUpdate(() =>
                {
                    if (pageRT.localEulerAngles.y >= 90)
                    { 
                        pageCG.alpha = 1;
                        numCG.alpha = 0;
                        numFlipCG.alpha = 1;
                        pageRT.SetSiblingIndex(siblingIndex);
                    }
                })
                    .OnComplete(() =>
                {
                    pageCG.blocksRaycasts = true;
                });
                break;
            case FlipSide.Right:

                pageRT.DORotate(new Vector3(0, 0, 0), time)
                    .SetDelay(delay)
                    .From(new Vector3(0, 180, 0))
                    .OnUpdate(() =>
                {
                    if (pageRT.localEulerAngles.y <= 90)
                    {
                        pageCG.alpha = 0;
                        numCG.alpha = 1;
                        numFlipCG.alpha = 0;
                        pageRT.SetAsLastSibling();
                    }
                })
                    .OnComplete(() =>
                {
                    pageCG.blocksRaycasts = false;
                });
                break;
        }
    }

}
