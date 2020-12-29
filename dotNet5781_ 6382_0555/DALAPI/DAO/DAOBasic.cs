namespace DALAPI.DAO
{
    public class DAOBasic
    {
        public int Id { get; set; }

        public virtual bool IsRunningId => true;
        public bool IsDeleted { get; set; }
    }
}