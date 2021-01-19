namespace BL.BO
{
    public class BOUser
    {
        internal int id;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsManager { get; set; }

        public int Id { get => id; }
    }

}
