using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace ZeroWaste.SharePortal
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            // TODO: Configure your bundles here...
            // Please read http://getcassette.net/documentation/configuration

            // This default configuration treats each file as a separate 'bundle'.
            // In production the content will be minified, but the files are not combined.
            // So you probably want to tweak these defaults!
            //bundles.AddPerIndividualFile<StylesheetBundle>("Content");
            //bundles.AddPerIndividualFile<ScriptBundle>("Scripts");

            // To combine files, try something like this instead:
            //   bundles.Add<StylesheetBundle>("Content");
            // In production mode, all of ~/Content will be combined into a single bundle.

            // If you want a bundle per folder, try this:
            //   bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
            // Each immediate sub-directory of ~/Scripts will be combined into its own bundle.
            // This is useful when there are lots of scripts for different areas of the website.

            bundles.Add<StylesheetBundle>("Content",
                "bootstrap.css",
                "bootstrap-responsive.css",
                "styles.css");

            bundles.Add<ScriptBundle>("Scripts",
                "modernizr.custom.22007.js",
                "retina.js",
                "jquery.validate.js",
                "jquery.validate.unobtrusive.js",
                "jquery.validate.unobtrusive-custom-for-bootstrap.js",
                "jquery.textchange.min.js");

            // Creates a bundle reference for parts.
            bundles.Add<ScriptBundle>("Scripts/Explore", "part.js");

            bundles.Add<ScriptBundle>("Scripts/Map", "infobox.js", "part.js");

            // Google map js reference.
            bundles.AddUrlWithAlias<ScriptBundle>("//maps.google.com/maps/api/js?sensor=false", "googlemap");
            
            bundles.AddUrlWithAlias<ScriptBundle>("//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-4f6c0d2a6f20faa7", "addthis");
        }
    }
}