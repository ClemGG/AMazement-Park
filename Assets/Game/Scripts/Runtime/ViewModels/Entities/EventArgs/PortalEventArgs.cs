namespace Project.ViewModels.Entities.EventArgs
{
    /// <summary>
    /// Des infos sur le portail lorsque le joueur entre en collision avec lui.
    /// </summary>
    public class PortalEventArgs : ItemEventArgs
    {
        #region Public Fields

        /// <summary>
        /// Le portail est-il déverrouillé ?
        /// </summary>
        public bool IsUnlocked { get; set; }

        #endregion

        #region Constructeurs

        public PortalEventArgs(bool isUnlocked)
        {
            IsUnlocked = isUnlocked;
        }

        #endregion
    }
}