﻿namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web.UI;

    public class HiPage : Page
    {
       static readonly Regex viewStateRegex = new Regex("<div>(\\s+)<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<data>.*?)\" />(\\s+)</div>(\\r\\n)+", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline);

        protected override void Render(HtmlTextWriter writer)
        {
            if (HiContext.Current.IsUrlReWritten)
            {
                if (writer is Html32TextWriter)
                {
                    writer = new FormFixerHtml32TextWriter(writer.InnerWriter);
                }
                else
                {
                    writer = new FormFixerHtmlTextWriter(writer.InnerWriter);
                }
            }
            if (EnableViewState)
            {
                base.Render(writer);
            }
            else
            {
                using (StringWriter writer2 = new StringWriter())
                {
                    using (HtmlTextWriter writer3 = new HtmlTextWriter(writer2))
                    {
                        base.Render(writer3);
                        string input = writer2.ToString();
                        Match match = viewStateRegex.Match(input);
                        if (match.Success)
                        {
                            input = input.Remove(match.Index, match.Length);
                        }
                        writer.Write(input);
                    }
                }
            }
        }
    }
}

