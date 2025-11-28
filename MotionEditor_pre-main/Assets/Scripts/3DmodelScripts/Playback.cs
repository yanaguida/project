using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum ModelState
{
    Play,
    Stop,
    Restart
}

public class Playback : MonoBehaviour
{
    public Redline redline;
    public GameObject StartText;
    public GameObject StopText;
    public GameObject RestartText;
    private Button targetButton;
    private ColorBlock cb;
    private Coroutine redlineCoroutine;
    private Coroutine buttonCoroutine;
    [SerializeField] private Audio singing;
    [SerializeField] private ModelState modelstate = ModelState.Play;
    private List<IIcon> allIcon = new List<IIcon>();
    private float t = 0;

    public void Awake()
    {
        if (redline == null) Debug.Log("Playbackでredlineがnull");
        targetButton = GetComponent<Button>();
        cb = targetButton.colors;
        SwitchText(0);
        SwitchColor(true);
    }

    void Update()
    {
        if (modelstate == ModelState.Stop)
            t += Time.deltaTime;
    }

    public void OnClick()
    {
        if (modelstate == ModelState.Play)
        {
            allIcon = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IIcon>().ToList();
            StartPlayback(CalculateLastLaneTime());
        }
        else if (modelstate == ModelState.Stop)
        {
            StopPlayback();
        }
        else if (modelstate == ModelState.Restart)
        {
            allIcon = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IIcon>().ToList();
            RestartPlayback(t, CalculateLastLaneTime(), CalculateLastLaneTime() - t);
        }
    }

    private void RestartPlayback(float time, float d, float a)
    {
        foreach (var icon in allIcon)
        {
            icon.ExecuteAction(time);
        }
        modelstate = ModelState.Stop;
        redlineCoroutine = StartCoroutine(redline.StartRedline(d));
        buttonCoroutine = StartCoroutine(SwitchButton(a));
        SwitchText(1);
    }

    private void StartPlayback(float d)
    {
        modelstate = ModelState.Stop;
        foreach (var icon in allIcon)
        {
            icon.ExecuteAction(0);
        }

        redlineCoroutine = StartCoroutine(redline.StartRedline(d));

        buttonCoroutine = StartCoroutine(SwitchButton(d));
    }

    private void StopPlayback()
    {
        modelstate = ModelState.Restart;
        foreach (var icon in allIcon)
        {
            if (icon.GetPartType() == PartType.Singing) singing.Pause();
            icon.StopCoroutine();
        }
        StopCoroutine(redlineCoroutine);
        StopCoroutine(buttonCoroutine);
        SwitchText(2);
        SwitchColor(true);
    }

    private float CalculateLastLaneTime()
    {
        float maxTime = 0f;
        foreach (var icon in allIcon)
        {
            if (icon.GetIconState() == IconState.OnLane)
            {
                icon.SetData();
                maxTime = Mathf.Max(maxTime, icon.GetStart() + icon.GetTime());
            }
        }
        return maxTime;
    }

    public ModelState GetModelState() => modelstate;

    private IEnumerator SwitchButton(float time)
    {
        SwitchText(1);
        SwitchColor(false);
        yield return new WaitForSeconds(time);
        Initialize();
    }

    public void Initialize()
    {
        modelstate = ModelState.Play;
        t = 0;
        SwitchText(0);
        SwitchColor(true);
    }

    private void SwitchText(int j)
    {
        if (j % 3 == 0)
        {
            StartText.SetActive(true);
            StopText.SetActive(false);
            RestartText.SetActive(false);
        }
        else if (j % 3 == 1)
        {
            StartText.SetActive(false);
            StopText.SetActive(true);
            RestartText.SetActive(false);
        }
        else
        {
            StartText.SetActive(false);
            StopText.SetActive(false);
            RestartText.SetActive(true);
        }
    }

    private void SwitchColor(bool i)
    {
        if (i)
        {
            cb.normalColor = new Color(0.78f, 0.90f, 0.98f);
            cb.highlightedColor = new Color(0.5f, 0.7f, 1.0f);
            cb.pressedColor = new Color(0.5f, 0.7f, 1.0f);
            cb.selectedColor = new Color(0.5f, 0.7f, 1.0f);
            cb.disabledColor = new Color(0.5f, 0.7f, 1.0f);
            targetButton.colors = cb;
        }
        else
        {
            cb.normalColor = new Color(1f, 0.75f, 0.71f);
            cb.highlightedColor = new Color(1f, 0.75f, 0.71f);
            cb.pressedColor = new Color(1f, 0.75f, 0.71f);
            cb.selectedColor = new Color(1f, 0.75f, 0.71f);
            cb.disabledColor = new Color(1f, 0.75f, 0.71f);
            targetButton.colors = cb;
        }
    }
}