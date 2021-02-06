namespace BL.BO
{
    /// <summary>
    /// user data
    /// </summary>
    public class BOUser
    {
        /// <summary>
        /// the user id (can't set it out of bl)
        /// </summary>
        internal int id;
        /// <summary>
        /// the user name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// the user password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// is the user manager
        /// </summary>
        public bool IsManager { get; set; }
        /// <summary>
        /// the user id getter
        /// </summary>
        public int Id { get => id; }
    }

}
