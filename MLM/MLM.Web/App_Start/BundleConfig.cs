using System;
using System.Web;
using System.Web.Optimization;
using MLM.Util;
namespace MLM.Web
{
    public class BundleConfig
    {

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);


            #region "Default"
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap_min.js",
                       
                        "~/Scripts/jqBootstrapValidation.js"

                        )
                        );

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/mvcapp").Include(
                        "~/Scripts/mvcApp/mainmvcapp.js",
                         "~/Scripts/mvcApp/Topachivers.js",
                         "~/Scripts/mvcApp/News.js"
                      ));

            
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));


            bundles.Add(new StyleBundle("~/Content/Bootstrap/css")
   .IncludeDirectory("~/Content/Bootstrap/css", "*.css", true));
            bundles.Add(new StyleBundle("~/Content/Bootstrap/asset/css")
  .IncludeDirectory("~/Content/Bootstrap/asset/css", "*.css", true));
            bundles.Add(new StyleBundle("~/Content/Bootstrap/css/colors")
 .IncludeDirectory("~/Content/Bootstrap/css/colors", "*.css", true));

            #endregion



            var scriptBundle = new ScriptBundle("~/bundles/MLMJs");
            scriptBundle.ConcatenationToken = string.Format(";{0}", Environment.NewLine);
            bundles.Add(scriptBundle

                .Include(

                        /////ANGULAR 1.2//////
                        "~/Scripts/angular/angular.js",
                        "~/Scripts/angular/angular-touch.js",
                        "~/Scripts/angular/angular-cookies.js",
                        "~/Scripts/angular/angular-resource.js",
                        "~/Scripts/angular/angular-route.js",
                        "~/Scripts/angular/angular-sanitize.js",
                        "~/Scripts/angular/angular-animate.js",
                        "~/Scripts/angular/ui-grid.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/underscore.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/jlinq.min.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/angular-cache.js",
                        "~/Scripts/deferredWithUpdate.js"
                //"~/Scripts/sonic.js"
                        )
                       .IncludeDirectory("~/App/components", "*.js", true)
                       .LocalizationJs("en")
            );

            bundles.Add(new StyleBundle("~/bundles/mlmTheme")
               
                            .Include("~/App/styles/main.css")
                );

            

            bundles.Add(new StyleBundle("~/Content/uigridcss")
                .Include("~/Content/uigrid/ui-grid.css"));

            // If we want to bundle without minification, we need to remove all the transform objects
            foreach (var bundle in BundleTable.Bundles)
                bundle.Transforms.Clear();

            // Do not explicitly turn on optimization, leave it to debug setting in web.config
            //BundleTable.EnableOptimizations = true;

          
                bundles.Add(new ScriptBundle("~/javascripts/layout")
    .IncludeDirectory("~/Content/Bootstrap","*.js",true));


        }

        /// <summary>
        /// Add whatever we don't need to add to the bundles.
        /// </summary>
        /// <param name="ignoreList">Bundles ignore lise</param>
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList != null)
            {
                ignoreList.Ignore("*.intellisense.js");
                ignoreList.Ignore("*-vsdoc.js");
                ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
                //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
                ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
            }
        }


        

    }
}