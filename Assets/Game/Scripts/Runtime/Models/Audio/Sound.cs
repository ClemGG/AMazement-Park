using UnityEngine;

namespace Project.Models.Audio
{
    /// <summary>
    /// Paramètres du son
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        #region Public Fields

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public AudioClip AudioClip { get; set; }

        [field: SerializeField]
        public float Volume { get; set; } = .2f;

        [field: SerializeField]
        public bool IsSound3D { get; set; } = false;

        [field: SerializeField]
        public bool Loop { get; set; } = false;

        #endregion


        #region Public Methods

#if UNITY_EDITOR

        public void OnValidate()
        {
            if (string.IsNullOrEmpty(Name) && AudioClip != null)
            {
                Name = AudioClip.name;
            }
        }

#endif

        #endregion
    }
}