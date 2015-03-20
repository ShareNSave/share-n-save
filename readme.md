#Share N Save

Share N Save is a website template created by [Beatwave](http://www.beatwave.com.au) and [Lithe](http://www.lithe.net.au). It provides a powerful search capability with the ability to display results in both list and Google map views. It also allows listing content to be easily managed via a comprehensive admin area. This area allows subscribers to manage their own content. Its clean stylish design gives you powerful core features:
*	Responsive
* Search displays results in both Google map and list form
* Allows subscribers to manage their own content

# Installation requirements
Share N Save is provided as an Visual Studio ASP.Net project, as such visual studio is required to build the project. Microsoft offers a free version of Visual Studio called Community Edition, which can be used to build Share N Save. The Visual Studio Community Edition can be downloaded [here](https://www.visualstudio.com/products/visual-studio-community-vs).

Share N Save is built on top of the open source [N2CMS](http://n2cms.com) platform, the source code for N2CMS can be found on [GitHub](http://github.com/n2cms/n2cms). N2CMS is built on the Microsoft ASP.Net platform and requires a hosting environment that supports ASP.Net application, i.e. Windows Server with IIS and SQL Server, more detailed server requirements can be found [here](https://n2cmsdocs.atlassian.net/wiki/display/N2CMS/Server+Requirements) in the N2 documentation.

# Getting it up and running
* First check out a copy of the Share N Save source code and open the solution file within Visual Studio, then build the project. The solution should automatically download all required dependencies through NuGet.
* Once you have successfully build the project, the next step is to run the application and go through the N2CMS installation wizard. The installation wizard will take you through configuring the database connection and admin account for the CMS so the application is ready to be used, instructions for the wizard can be found [here](https://n2cmsdocs.atlassian.net/wiki/display/N2CMS/The+N2+Installation+Wizard).
