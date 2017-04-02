
namespace Ayatta.Web
{
    public static class WebSite
    {
        private static readonly string MainDomain = "ayatta.com";
        public static string Main { get { return string.Format("http://{0}.{1}", "www", MainDomain); } }
        public static string Cart { get { return string.Format("http://{0}.{1}", "cart", MainDomain); } }
        public static string Item { get { return string.Format("http://{0}.{1}", "item", MainDomain); } }
        public static string Static { get { return string.Format("http://{0}.{1}", "static", MainDomain); } }
        public static string Game { get { return string.Format("http://{0}.{1}", "game", MainDomain); } }
        public static string Passport { get { return string.Format("http://{0}.{1}", "passport", MainDomain); } }
        public static string Pay { get { return string.Format("http://{0}.{1}", "pay", MainDomain); } }
        public static string My { get { return string.Format("http://{0}.{1}", "my", MainDomain); } }

        public static string Seller { get { return string.Format("http://{0}.{1}", "seller", MainDomain); } }
        public static string Logistics { get { return string.Format("http://{0}.{1}", "logistics", MainDomain); } }

        public static string GenerateCartOperationUrl(int itemId, int skuId, int quantity = 0, int max = int.MaxValue)
        {
            if (quantity > 0)
            {
                if (quantity > max)
                {
                    return string.Format("{0}/cart?itemId={1}&skuId={2}&quantity={3}", Cart, itemId, skuId, quantity - 1);
                }
                return string.Format("{0}/cart?itemId={1}&skuId={2}&quantity={3}", Cart, itemId, skuId, quantity);
            }
            return string.Format("{0}/cart?itemId={1}&skuId={2}&opt=1", Cart, itemId, skuId);
        }
    }
}