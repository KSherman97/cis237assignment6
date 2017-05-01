/***
* Kyle Sherman
* CIS237 ASSIGNMENT 6
* WINE web-application
* The purpose of this application is 
* to enhance my skills with MVC systems in ASP.net
* using the razer templating engine, 
* router web framwork.
***/

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cis237Assignment6.Startup))]
namespace cis237Assignment6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
