namespace Jobz.WebUi
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.2.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include("~/Scripts/vue-dev.js"));

            //https://wffranco.github.io/vue-strap/#getting-started
            //Be sure to match up the versions if you change anything here.
            bundles.Add(new ScriptBundle("~/bundles/vue-strap").Include("~/Scripts/vue-strap.js"));

            //still needed for the nav drop down, should replace with slide in 
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/JLinq").Include("~/Scripts/JLinq.js"));
        }
    }
}
