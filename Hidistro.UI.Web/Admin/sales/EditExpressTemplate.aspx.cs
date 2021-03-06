﻿using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ExpressTemplates)]
    public partial class EditExpressTemplate : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            int result = 0;

            string expressName = Page.Request.QueryString["ExpressName"];

            string xmlFile = Page.Request.QueryString["XmlFile"];

            if (!int.TryParse(Page.Request.QueryString["ExpressId"], out result))
            {
                GotoResourceNotFound();
            }
            else if (!((!string.IsNullOrEmpty(expressName) && !string.IsNullOrEmpty(xmlFile)) && xmlFile.EndsWith(".xml")))
            {
                GotoResourceNotFound();
            }

        }

    }


}


