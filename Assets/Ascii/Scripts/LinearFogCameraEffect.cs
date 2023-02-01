///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Ascii - Image Effect.
// Copyright (c) Digital Software/Johan Munkestam. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//From ASCIIZarkov's ASCII Shader:
//http://www.digitalsoftware.se/community/thread-19.html


using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Project.Shaders
{
    /// <summary>
    /// Linear Fog - Image Effect.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Linear Fog")]
    public sealed class LinearFogCameraEffect : MonoBehaviour
    {
        #region Public Properties

        [field: SerializeField]
        private Shader Shader { get; set; }

        [field: SerializeField]
        private bool Fog { get; set; } = false;

        [field: SerializeField, Min(0f)]
        private float FogDensity { get; set; } = 15f;

        [field: SerializeField]
        private Color FogColor { get; set; } = Color.black;

        #endregion

        #region Private Properties

        private Material _materialToUse;
        private Material _materialToEdit;
        private Camera _camera;

        #endregion

#if UNITY_EDITOR

        private void OnValidate()
        {
            //Pour éviter que la prefab ne change les paramètres au lieu de l'instance
            if (!PrefabModeIsActive(gameObject))
            {
                if (enabled && gameObject.activeInHierarchy)
                {
                    Awake();
                }
            }
        }

        bool PrefabModeIsActive(GameObject gameobject)
        {
            bool isObjInPrefabMode = PrefabStageUtility.GetPrefabStage(gameobject) != null;
            bool isPrefabModeActive = PrefabStageUtility.GetCurrentPrefabStage() != null;
            return isObjInPrefabMode || isPrefabModeActive;
        }

#endif


        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _camera.depthTextureMode = DepthTextureMode.DepthNormals;

            if (!Shader)
            {
                Debug.LogError("Linear Fog shader not found.");

                this.enabled = false;
                return;
            }


            if (!_materialToEdit)
            {
                _materialToEdit = new Material(Shader);
            }
        }

        /// <summary>
        /// Check.
        /// </summary>
        private void OnEnable()
        {
            _materialToUse = _materialToEdit;
        }

        /// <summary>
        /// Destroy the material.
        /// </summary>
        private void OnDisable()
        {
            _materialToUse = null;
        }

        private void SetFogSettings()
        {
            //Depth
            _materialToEdit.SetFloat(@"fogDensity", FogDensity);
            _materialToEdit.SetFloat(@"useFog", Fog ? 1f : 0f);
            _materialToEdit.SetColor(@"fogColor", FogColor);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_materialToUse)
            {
                if (!Application.isPlaying)
                {
                    SetFogSettings();
                }

                Graphics.Blit(source, destination, _materialToUse, 0);
            }
        }
    }
}
