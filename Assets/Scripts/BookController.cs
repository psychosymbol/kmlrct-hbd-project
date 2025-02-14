using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
    public static BookController instance;
    Checkmark checkMark;
    public List<PageController> pages = new List<PageController>();

    private void Awake()
    {
        if (!instance) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        currentPage = 0;
        checkMark = new Checkmark(pages.Count * 4); //1 page has 4 characters/buttons
    }

    public RectTransform bookButtonRT;
    public RectTransform bookGroupRT;
    public CanvasGroup bookGroupCG;
    public RectTransform coverRT;
    public RectTransform contentRT;
    public CanvasGroup frontCoverCG;
    public CanvasGroup bgCG;

    public void OpenBook()
    {
        bookButtonRT.GetComponent<Animator>().enabled = false;
        bookButtonRT.DOScale(Vector3.zero, .5f);
        bgCG.DOFade(1, .25f).From(0).OnComplete(() =>
        {
            bookGroupRT.localScale = new Vector3(.1f, .1f, 1);
            bookGroupCG.alpha = 1;
            bookGroupCG.interactable = true;
            bookGroupCG.blocksRaycasts = true;

            bookGroupRT.DOScale(Vector3.one, .5f);
            bookGroupRT.DOLocalMove(Vector3.zero, .5f).From(bookButtonRT.localPosition).OnComplete(() =>
            {
                //frontCoverCG.DOFade(1,0).SetDelay(.5f);
                coverRT.DORotate(new Vector3(0, 180, 0), 1f).From(new Vector3(0, 0, 0)).OnUpdate(() =>
                {
                    if (coverRT.localEulerAngles.y >= 90) frontCoverCG.alpha = 1;
                }).OnComplete(() =>
                {
                    bgCG.interactable = true;
                    bgCG.blocksRaycasts = true;
                    contentRT.SetAsLastSibling();
                });
            });
        });
    }

    public void CloseBook()
    {
        bgCG.interactable = false;
        bgCG.blocksRaycasts = false;

        PageController.FlipSide side = PageController.FlipSide.Right;

        for (int i = currentPage - 1; i > -1; i--)
        {
            pages[i].FlipPage(.5f, 0, 0, side);
        }

        coverRT
            .DORotate(new Vector3(0, 0, 0), .5f)
            .From(new Vector3(0, 180, 0))
            .OnUpdate(() =>
        {
            if (coverRT.localEulerAngles.y <= 90) 
            { 
                frontCoverCG.alpha = 0;
                coverRT.SetAsLastSibling();
            }
        })
            .OnComplete(() =>
        {
            bookGroupRT.DOScale(new Vector3(0, 0, 1), .25f);
            bookGroupRT.DOLocalMove(bookButtonRT.localPosition, .25f)
            .From(Vector3.zero)
            .OnComplete(() =>
            {
                bookGroupCG.alpha = 0;
                bookGroupCG.interactable = false;
                bookGroupCG.blocksRaycasts = false;

                bgCG.DOFade(0, .25f).From(1)
                .OnComplete(() =>
                {
                    bookButtonRT.DOScale(Vector3.one, .25f)
                    .OnComplete(() =>
                    {
                        bookButtonRT.GetComponent<Animator>().enabled = true;
                    });
                });
            });
        });
        currentPage = 0;
    }

    private int currentPage = 0;

    public void FlipBookToPage(int pageNum)
    {
        PageController.FlipSide side = PageController.FlipSide.Left;
        float time = 1f;
        float delayIncrement = .1f;
        float delay = 0;
        if (pageNum == currentPage) return;
        if (pageNum > currentPage)
        {
            side = PageController.FlipSide.Left;

            for (int i = currentPage; i < pageNum; i++)
            {
                pages[i].FlipPage(time, delay,i, side);
                delay += delayIncrement;
            }
        }
        else if (pageNum < currentPage)
        {
            side = PageController.FlipSide.Right;

            for (int i = currentPage - 1; i > pageNum - 1; i--)
            {
                pages[i].FlipPage(time, delay, 0, side);
                delay += delayIncrement;
            }
        }

        currentPage = pageNum;
    }
}

public class Checkmark
{
    public List<bool> isChecks = new List<bool>();
    public Checkmark(int markCount)
    {
        for (int i = 0; i < markCount; i++)
        {
            isChecks.Add(false);
        }
    }

    public void CheckButton(int index, bool isTrue)
    {
        isChecks[index] = isTrue;
    }
}
