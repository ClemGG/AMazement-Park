using Project.Core;
using Project.Models.Audio;
using UnityEngine;

namespace Project.ViewModels.Audio
{

    /// <summary>
    /// Gère les effets sonores et la musique
    /// </summary>
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        #region Public Fields

        [field: SerializeField]
        private Sound[] SoundEffects { get; set; }

        [field: SerializeField]
        private Sound[] BGMs { get; set; }

        #endregion

        #region Mono

#if UNITY_EDITOR

        private void OnValidate()
        {
            for (int i = 0; i < SoundEffects.Length; i++)
            {
                SoundEffects[i].OnValidate();
            }
            for (int i = 0; i < BGMs.Length; i++)
            {
                BGMs[i].OnValidate();
            }
        }

#endif

        #endregion


        #region Public Methods
        #endregion
    }
}