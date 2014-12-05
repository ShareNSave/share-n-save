
using N2;
using N2.Engine;
using N2.Plugin;
using N2.Security;
using N2.Web;

namespace ZeroWaste.SharePortal.Registrations
{
    [Service]
    public class PermissionDeniedHandler : IAutoStart
    {
        private readonly ISecurityEnforcer securityEnforcer;
        private readonly IUrlParser parser;
        private readonly IWebContext context;

        public PermissionDeniedHandler(ISecurityEnforcer securityEnforcer, IUrlParser parser, IWebContext context)
        {
            this.securityEnforcer = securityEnforcer;
            this.parser = parser;
            this.context = context;
        }

        void securityEnforcer_AuthorizationFailed(object sender, CancellableItemEventArgs e)
        {
            e.Cancel = true;
            context.HttpContext.Response.Redirect(Url.Parse("~/Account/Login").AppendQuery("returnUrl", context.Url.LocalUrl));
        }

        #region IStartable Members

        public void Start()
        {
            securityEnforcer.AuthorizationFailed += securityEnforcer_AuthorizationFailed;
        }

        public void Stop()
        {
            securityEnforcer.AuthorizationFailed -= securityEnforcer_AuthorizationFailed;
        }

        #endregion
    }
}