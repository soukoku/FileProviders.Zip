using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cadru.TagHelpers
{
    // modified from http://scottdorman.github.io/2016/08/01/input-tag-helper-for-bootstrap-navigation-links/

    [OutputElementHint("li")]
    public class BootstrapNavLinkTagHelper : AnchorTagHelper
    {
        public BootstrapNavLinkTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        private bool ShouldBeActive()
        {
            var routeData = ViewContext.RouteData.Values;
            var result = false;

            if (!string.IsNullOrEmpty(Page))
            {
                // razor page
                var currentPage = routeData["page"] as string;
                result = string.Equals(Page, currentPage, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // mvc
                var currentController = routeData["controller"] as string;
                var currentAction = routeData["action"] as string;
                if (!String.IsNullOrWhiteSpace(Controller) && !String.IsNullOrWhiteSpace(Action))
                {
                    result = String.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase) &&
                        String.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase);
                }
                else if (!String.IsNullOrWhiteSpace(Action))
                {
                    result = String.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase);
                }
                else if (!String.IsNullOrWhiteSpace(Controller))
                {
                    result = String.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase);
                }
            }
            return result;
        }

        private void MakeActive(TagHelperOutput output)
        {
            TagHelperAttribute classAttribute;
            if (output.Attributes.TryGetAttribute("class", out classAttribute))
            {
                output.Attributes.SetAttribute("class", classAttribute.Value + " active");
            }
            else
            {
                output.Attributes.Add(new TagHelperAttribute("class", "active"));
            }
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);


            var childContent = await output.GetChildContentAsync();
            var content = childContent.GetContent();
            output.TagName = "li";

            var href = output.Attributes.FirstOrDefault(a => a.Name == "href");
            if (href != null)
            {
                var tagBuilder = new TagBuilder("a");
                tagBuilder.Attributes.Add("href", href.Value.ToString());
                tagBuilder.InnerHtml.AppendHtml(content);

                output.Content.SetHtmlContent(tagBuilder);
                output.Attributes.Remove(href);
            }
            else
            {
                output.Content.SetHtmlContent(content);
            }

            if (ShouldBeActive())
            {
                MakeActive(output);
            }
        }
    }
}