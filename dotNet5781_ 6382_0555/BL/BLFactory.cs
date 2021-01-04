namespace BL
{
    public class BLFactory
    {
        public static IBL API
        {
            get
            {
                return BL.Instance;
            }
        }

    }
}
