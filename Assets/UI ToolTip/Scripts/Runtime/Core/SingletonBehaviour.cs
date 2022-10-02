using UnityEngine;

namespace Tooltip.Core
{

    /// <summary>
    /// Un singleton pour MonoBehaviours 
    /// regroupant toutes les fonctions relatives à l'instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        #region Static Fields

        public static T Instance { get; private set; }

        #endregion

        #region Public Fields

        [field: Header("Singleton")]
        [field: SerializeField]
        public bool PersistBetweenScenes { get; private set; } = true;

        #endregion

        #region Mono

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = (T)this;

            if (PersistBetweenScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
            OnAwake();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// A surcharger si on veut utiliser Awake
        /// </summary>
        protected virtual void OnAwake() { }

        #endregion
    }
}