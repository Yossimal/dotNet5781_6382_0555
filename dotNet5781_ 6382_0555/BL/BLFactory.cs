namespace BL
{
    public class BLFactory
    {
        /// <summary>
        /// get the instance of BL
        /// </summary>
        public static IBL API
        {
            get
            {
                return BL.Instance;
            }
        }

    }
}
