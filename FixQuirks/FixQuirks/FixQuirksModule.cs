using System;
using System.Configuration;
using System.Web;

namespace KeesTalksTech.WebUI.ExtendedModules
{
	/// <summary>
	/// Module that will detect an IE browser and add the right X-UA-Compatible header.
	/// </summary>
	public class FixQuirksModule : IHttpModule
	{
		private static bool? useEdge;

		/// <summary>
		/// Gets a value indicating whether to use an edge header.
		/// </summary>
		/// <value>
		///   <c>true</c> if edge should be used; otherwise, <c>false</c>.
		/// </value>
		private static bool UseEdge
		{
			get
			{
				if (useEdge == null)
				{
					bool result = false;
					Boolean.TryParse(ConfigurationManager.AppSettings["FixQuirksModule.UseEdge"], out result);
					useEdge = result;
				}

				return useEdge.GetValueOrDefault();
			}
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the module that implements 
		/// <see cref="T:System.Web.IHttpModule"/>.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access 
		/// to the methods, properties, and events common to all application objects within an 
		/// ASP.NET application</param>
		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(AddResponseHeader);
		}

		/// <summary>
		/// Adds the response header.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void AddResponseHeader(object sender, EventArgs e)
		{
			HttpApplication application = sender as HttpApplication;
			var browser = application.Request.Browser;

			if (browser.Browser == "IE")
			{
				if (application.Response.Headers["X-UA-Compatible"] == null)
				{
					if (UseEdge)
					{
						application.Response.Headers.Add("X-UA-Compatible", "IE=edge");
					}
					else if (browser.MajorVersion > 7)
					{
						application.Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE" + browser.MajorVersion);
					}
					else if (browser.MajorVersion == 7)
					{
						string userAgent = application.Request.UserAgent;

						if (userAgent.Contains("Trident/7.0"))
						{
							application.Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE11");
						}
						else if (userAgent.Contains("Trident/6.0"))
						{
							application.Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE10");
						}
						else if (userAgent.Contains("Trident/5.0"))
						{
							application.Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE9");
						}
						else if (userAgent.Contains("Trident/4.0"))
						{
							application.Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE8");
						}
						else
						{
							application.Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE7");
						}
					}
				}
			}
		}
	}
}