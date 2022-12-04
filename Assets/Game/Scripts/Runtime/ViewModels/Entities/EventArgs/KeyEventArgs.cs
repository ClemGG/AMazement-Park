namespace Project.ViewModels.Entities.EventArgs
{
    /// <summary>
    /// Des infos sur la clé lorsque le joueur entre en collision avec elle.
    /// </summary>
    public class KeyEventArgs : ItemEventArgs
    {
        public KeyEventArgs() : base(0)
        {
        }
    }
}