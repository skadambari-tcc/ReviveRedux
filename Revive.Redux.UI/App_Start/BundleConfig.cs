using System.Web;
using System.Web.Optimization;

namespace Revive.Redux.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // The jQuery bundle
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //           "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-1.10.2.min.js"
                            ));

            // For data annotation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                              "~/Scripts/jquery.validate*",
                               "~/Scripts/jquery.unobtrusive*"));

             //The Kendo JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Kendo/JS/kendo.web.min.js",                    
                    "~/Kendo/JS/kendo.kendo.all.min.js",
                    "~/Kendo/JS/kendo.aspnetmvc.min.js"
                    ));

           

            // The validationEngine JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/validationEngine").Include(
                   "~/Scripts/jquery.validationEngine.js",
                   "~/Scripts/jquery.validationEngine-en.js"));

            // The others js bundle
            bundles.Add(new ScriptBundle("~/bundles/others").Include(
                            "~/Scripts/toastr.js",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/jquery.maskedinput.min.js",
                            "~/Scripts/redux.common.js",
                            "~/Scripts/bootbox.min.js",
                            "~/Scripts/jquery-confirm.js"
                          
                             ));

            // The Kendo CSS bundle
            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                    "~/Kendo/CSS/kendo.common.min.css",
                    "~/Kendo/CSS/kendo.default.min.css"                    
                       // "~/Kendo/CSS/kendo.dataviz.min.css",
                       // "~/Kendo/CSS/kendo.dataviz.default.min.css"
                     ));
            // The others CSS bundle
            bundles.Add(new StyleBundle("~/Files/others").Include(
                "~/Scripts/css/toastr.css",    
                "~/Scripts/css/component.css",
                      "~/Scripts/css/main.css"
                      ));
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new StyleBundle("~/xontent/css").Include(

                      "~/Scripts/css/bootstrap.css"
                      ));

            // Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
            bundles.IgnoreList.Clear();


            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);

        }
    }
}
