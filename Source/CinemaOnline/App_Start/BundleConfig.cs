using System.Web;
using System.Web.Optimization;

namespace CinemaOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/gozha-nav.css",
                      "~/Content/css/external/jquery.selectbox.css",
                      "~/Content/rs-plugin/css/settings.css",
                      "~/Content/css/style3860.css?v=1"
                      ));
        }
    }
}
