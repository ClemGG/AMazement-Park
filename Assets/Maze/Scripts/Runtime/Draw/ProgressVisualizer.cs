using Project.Procedural.MazeGeneration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProgressVisualizer
{
    #region UI Fields
    private static Slider _progressFill;
    private static TextMeshProUGUI _progressText;
    private static GameObject _progressCanvas;

    private static Slider ProgressFill
    {
        get
        {
            if (!_progressFill) _progressFill = GameObject.Find("progress bar").GetComponent<Slider>();
            return _progressFill;
        }
    }
    private static TextMeshProUGUI ProgressText
    {
        get
        {
            if (!_progressText) _progressText = GameObject.Find("progress text").GetComponent<TextMeshProUGUI>();
            return _progressText;
        }
    }
    private static GameObject ProgressCanvas
    {
        get
        {
            if (!_progressCanvas) _progressCanvas = GameObject.Find("Progress canvas");
            return _progressCanvas;
        }
    }

    #endregion



    public void DisplayDrawProgress(GenerationProgressReport progress)
    {
        ProgressFill.value = progress.ProgressPercentage;
        ProgressText.text = progress.ToString();
    }

    public void DisplayGenerationProgress(GenerationProgressReport progress)
    {
        ProgressFill.value = progress.ProgressPercentage;
        ProgressText.text = progress.ToString();
    }

    public void Cleanup()
    {
        if (!ProgressCanvas) return;

        ProgressFill.value = 0f;
        ProgressText.text = "";
    }

    public void HideCanvas()
    {
        ProgressCanvas.SetActive(false);
    }
}
