using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace ZeroWaste.SharePortal
{
    public static class MembershipProviderExtensions
    {
        public static MembershipUser CreateUser(this MembershipProvider membershipProvider, string username, string password, string email)
        {
            return membershipProvider.CreateUser(username, password, email, null, null);
        }

        public static MembershipUser CreateUser(this MembershipProvider membershipProvider, string username, string password, string email, string passwordQuestion, string passwordAnswer)
        {
            MembershipCreateStatus status;
            var user = membershipProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, true, null, out status);
            if (user == null)
                throw new MembershipCreateUserException(status);
            
            return user;
        }
    }

}