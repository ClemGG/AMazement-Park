namespace Project.ViewModels.Entities.EventArgs
{
    public class ItemEventArgs : System.EventArgs
    {
        public ItemEventArgs(int iD)
        {
            ID = iD;
        }

        /// <summary>
        /// L'ID de l'objet dans la liste des objets actifs (0, 1 ou 2)
        /// </summary>
        public int ID { get; set; }
    }
}