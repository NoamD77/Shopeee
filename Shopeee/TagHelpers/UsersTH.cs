using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Shopeee.Areas.Identity;
using Shopeee.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shopeee.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "i-user")]
    public class UserTH : TagHelper
    {
        private UserManager<ApplicationUser> userManager;

        public UserTH(UserManager<ApplicationUser> UserManager)
        {
            userManager = UserManager;
        }

        [HtmlAttributeName("i-user")]
        public string User { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            ApplicationUser user = await userManager.FindByIdAsync(User);
            List<string> ListOfRoles = userManager.GetRolesAsync(user).Result.ToList();
            if (user != null)
            {
                foreach (var role in ListOfRoles)
                {
                    if (role != null)
                    {
                        names.Add(role);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Roles" : string.Join(", ", names));
        }
    }
}
