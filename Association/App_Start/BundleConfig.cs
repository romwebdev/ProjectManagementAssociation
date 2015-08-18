using System.Web.Optimization;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //add js autocomplete for parents
            bundles.Add(new ScriptBundle("~/bundles/autocompleteParents").Include(
                     "~/Scripts/autocomplete/autocomplete.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/fullCalendar").Include(
                "~/Scripts/fullcalendar/moment.min.js",     
                "~/Scripts/fullcalendar/*.js"
                      
                ));

            bundles.Add(new ScriptBundle("~/Content/datepickerCss").Include(
                        "~/Content/bootstrap-datepicker3.css"
                        //"~/Content/datepicker.css"
            ));
            
            bundles.Add(new ScriptBundle("~/bundles/datepickerJs").Include(
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/locales/bootstrap-datepicker.fr.min.js"
            ));
            
            bundles.Add(new ScriptBundle("~/bundles/jquery/otf").Include(
                        "~/Scripts/bootstrap.js",
                         "~/Scripts/main/*.js",
                         "~/Scripts/theme/*.js",
                        "~/Scripts/ui/*.js"
            ));
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/fullcalendar").Include(
                      "~/Content/fullcalendar/fullcalendar.css"
                      //"~/Content/fullcalendar/fullcalendar.print.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Themes/base/css").Include(
                      "~/Content/themes/base/css/jquery-ui.css",
                      "~/Content/themes/base/css/jquery-ui.theme.css",
                      "~/Content/themes/base/css/jquery-ui.structure.css"
          ));

            // Définissez EnableOptimizations sur False pour le débogage. Pour plus d'informations,
            // visitez http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
