using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField]
    private GameObject firstView;

    [SerializeField]
    private float fadeTimeValue = 1.0f;
    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
    private FadeType fadeType;
    private GameObject previousView;
    private GameObject nextView;
    private float alpha;
    private bool isAnimating;

    void Awake()
    {
        previousView = firstView;
    }

    void FixedUpdate()
    {
        if (isAnimating)
        {
            FadeAnimation();
        }
    }

    private void FadeAnimation()
    {
        switch (fadeType)
        {
            case FadeType.FadeIn:

                alpha += Time.deltaTime / (fadeTimeValue * 0.5f);
                SetAlpha(nextView, alpha);
                if (alpha >= 1.0f)
                {
                    previousView = nextView;
                    isAnimating = false;
                }

                break;
            case FadeType.FadeOut:

                alpha -= Time.deltaTime / (fadeTimeValue * 0.5f);
                SetAlpha(previousView, alpha);
                if (alpha <= 0.0f)
                {
                    previousView.SetActive(false);
                    nextView.SetActive(true);
                    fadeType = FadeType.FadeIn;
                }

                break;
            default:
                break;
        }
    }

    void SetAlpha(GameObject view, float alpha)
    {
        view.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void StartFadeTransition(GameObject nextView)
    {
        if (!previousView.GetComponent<CanvasGroup>())
        {
            previousView.AddComponent<CanvasGroup>();
        }

        if (!nextView.GetComponent<CanvasGroup>())
        {
            nextView.AddComponent<CanvasGroup>();
        }

        fadeType = FadeType.FadeOut;
        alpha = 1.0f;

        SetAlpha(previousView, 1.0f);
        SetAlpha(nextView, 0.0f);
        previousView.SetActive(true);
        nextView.SetActive(false);

        this.nextView = nextView;

        isAnimating = true;
    }
}