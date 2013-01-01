
namespace Business.Database
{
    public static class ShopData
    {
        public static shopDataContext DataContext
        {
            get { return new shopDataContext(); }
        }
    }
}