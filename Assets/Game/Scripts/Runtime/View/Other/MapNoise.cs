using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Génère un effet de brouillage sur la carte du dédale
/// </summary>
public class MapNoise : MonoBehaviour
{
    #region Variables Unity

    [SerializeField]
    private Image _img;

    [SerializeField]
    private float _defaultNoiseScale = 1f;

    [SerializeField]
    private float _defaultDurationSeconds = 5f;

    [SerializeField]
    private float _defaultPauseSeconds = .1f;

    [SerializeField]
    private Vector2Int _texSize = new(128, 128);

    #endregion

    #region Variables d'instance

    private Color[] _pixels;

    /// <summary>
    /// Pour éviter de relancer la Coroutine si elle est toujours active
    /// </summary>
    private Coroutine _co;

    #endregion

    #region Fonctions Unity

    private void Start()
    {
        _img.enabled = false;
    }

    #endregion

    #region Fonctions publiques

    [ContextMenu("Show Noise")]
    public void ShowNoise()
    {
        if(Application.isPlaying)
            ShowNoise(_defaultNoiseScale, _defaultPauseSeconds, _defaultDurationSeconds);
    }

    /// <summary>
    /// Génère en continu l'effet de brouillage sur une durée "<paramref name="durationSeconds"/>",
    /// avec une pause de <paramref name="pauseSeconds"/> entre chaque génération.
    /// </summary>
    /// <param name="noiseScale">La taille du brouillage</param>
    /// <param name="pauseSeconds">L'intervale de pause entre chaque génération</param>
    /// <param name="durationSeconds">La durée du brouillage</param>
    public void ShowNoise(float noiseScale, float pauseSeconds, float durationSeconds)
    {
        if(_co == null)
        {
            _pixels = new Color[_texSize.x * _texSize.y];
            GenerateNoise(_img, _texSize, _pixels, noiseScale);
            _co = StartCoroutine(GenerateNoiseOverTimeCo(_img, _texSize, _pixels, noiseScale, pauseSeconds, durationSeconds));
        }
    }

    #endregion

    #region Fonctions privées

    /// <summary>
    /// Génère en continu l'effet de brouillage sur une durée "<paramref name="durationSeconds"/>",
    /// avec une pause de <paramref name="pauseSeconds"/> entre chaque génération.
    /// </summary>
    /// <param name="img">L'image sur laquelle appliquer l'effet</param>
    /// <param name="noiseScale">La taille du brouillage</param>
    /// <param name="texSize">La taille de la texture</param>
    /// <param name="pixels">La liste de couleurs pour chaque pixel</param>
    /// <param name="pauseSeconds">L'intervale de pause entre chaque génération</param>
    /// <param name="durationSeconds">La durée du brouillage</param>
    private IEnumerator GenerateNoiseOverTimeCo(Image img, Vector2Int texSize, Color[] pixels, float noiseScale, float pauseSeconds, float durationSeconds)
    {
        float tDur = 0f;
        float tPause = 0f;
        img.enabled = true;

        while (tDur < durationSeconds)
        {
            tDur += Time.deltaTime;
            if (tPause < pauseSeconds)
            {
                tPause += Time.deltaTime;
                yield return null;
            }
            else
            {
                tPause = 0f;
                GenerateNoise(img, texSize, pixels, noiseScale);
            }
        }

        img.enabled = false;
        _co = null;
    }

    /// <summary>
    /// Génère l'effet de brouillage
    /// </summary>
    /// <param name="img">L'image sur laquelle appliquer l'effet</param>
    /// <param name="noiseScale">La taille du brouillage</param>
    /// <param name="texSize">La taille de la texture</param>
    /// <param name="pixels">La liste de couleurs pour chaque pixel</param>
    private void GenerateNoise(Image img, Vector2Int texSize, Color[] pixels, float noiseScale)
    {

        Texture2D tex = new(texSize.x, texSize.y);
        img.material.mainTexture = tex;

        for (int x = 0; x < texSize.x; x++)
        {
            for (int y = 0; y < texSize.y; y++)
            {
                float sample = Random.Range(0f, 1f);
                pixels[(int)y * texSize.x + (int)x] = new Color(sample, sample, sample);
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();

        // Obligatoire pour que l'image affiche la texture mise à jour
        img.SetMaterialDirty();
    }


    #endregion
}
