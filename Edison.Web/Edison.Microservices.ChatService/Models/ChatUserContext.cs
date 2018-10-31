using Edison.Core.Common;
using Edison.Core.Common.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Edison.ChatService.Models
{
    public class ChatUserContext : ChatUserModel
    {
        public static ChatUserContext FromClaims(IEnumerable<Claim> claims)
        {
            List<Claim> claimsList = claims.ToList();

            string id = claimsList.Find(p => p.Type == "emails")?.Value;
            if(string.IsNullOrEmpty(id))
                id = claimsList.Find(p => p.Type == "preferred_username")?.Value;

            var userContext = new ChatUserContext()
            {
                Id = $"dl_{id}",
                Name = claimsList.Find(p => p.Type == "name")?.Value,
                Role = GetUserRoleFromString("Admin")
            };

            return userContext;
        }

        public static ChatUserContext FromConversation(ConversationReference conversation)
        {
            return new ChatUserContext()
            {
                Id = conversation.User?.Id,
                Name = conversation.User?.Name,
                Role = GetUserRoleFromString(conversation.User?.Role)
            };
        }

        public void SetUserRoleAgainstAdminList(List<string> admins)
        {
            string userId = Id.ToLower();
            if (userId.StartsWith("dl_"))
                userId = userId.Substring(3);
            foreach (string admin in admins)
            {
                var adminToTest = admin.ToLower();
                if (adminToTest.StartsWith("*"))
                {
                    if (Regex.IsMatch(userId, CoreHelper.GetWildCardExpression(adminToTest)))
                    {
                        Role = ChatUserRole.Admin;
                        return;
                    }
                }
                else if (adminToTest == userId)
                {
                    Role = ChatUserRole.Admin;
                    return;
                }
            }
            Role = ChatUserRole.Consumer;
        }

        private static ChatUserRole GetUserRoleFromString(string roleStr)
        {
            Enum.TryParse(typeof(ChatUserRole), roleStr, out object role);
            if (role == null)
                role = ChatUserRole.Consumer;
            return (ChatUserRole)role;
        }
    }
}
