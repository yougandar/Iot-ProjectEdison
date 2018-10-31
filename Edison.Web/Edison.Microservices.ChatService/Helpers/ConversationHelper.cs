using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edison.ChatService.Helpers
{
    public static class ConversationHelper
    {

        /// <summary>
        /// Resolves the non-null channel account instance in the given conversation reference.
        /// </summary>
        /// <param name="conversationReference">The conversation reference whose channel account to resolve.</param>
        /// <param name="isBot">Will be true, if the conversation reference is associated with a bot. False otherwise.</param>
        /// <returns>The non-null channel account instance (user or bot) or null, if both are null.</returns>
        public static ChannelAccount GetChannelAccount(ConversationReference conversationReference, out bool isBot)
        {
            if (conversationReference?.User != null)
            {
                isBot = false;
                return conversationReference.User;
            }

            if (conversationReference?.Bot != null)
            {
                isBot = true;
                return conversationReference.Bot;
            }

            isBot = false;
            return null;
        }

        /// <summary>
        /// Resolves the non-null channel account instance in the given conversation reference.
        /// </summary>
        /// <param name="conversationReference">The conversation reference whose channel account to resolve.</param>
        /// <returns>The non-null channel account instance (user or bot) or null, if both are null.</returns>
        public static ChannelAccount GetChannelAccount(ConversationReference conversationReference)
        {
            return GetChannelAccount(conversationReference, out bool isBot);
        }

        /// <summary>
        /// Compares the conversation and channel account instances of the two given conversation references.
        /// </summary>
        /// <param name = "conversationReference1" ></ param >
        /// < param name="conversationReference2"></param>
        /// <returns>True, if the IDs match.False otherwise.</returns>
        public static bool Match(
            ConversationReference conversationReference1, ConversationReference conversationReference2)
        {
            if (conversationReference1 == null || conversationReference2 == null)
            {
                return false;
            }

            string conversationAccount1Id = conversationReference1.Conversation?.Id;
            string conversationAccount2Id = conversationReference2.Conversation?.Id;

            if (string.IsNullOrWhiteSpace(conversationAccount1Id) != string.IsNullOrWhiteSpace(conversationAccount2Id))
            {
                return false;
            }

            bool conversationAccountsMatch =
                (string.IsNullOrWhiteSpace(conversationAccount1Id)
                 && string.IsNullOrWhiteSpace(conversationAccount2Id))
                || conversationAccount1Id.Equals(conversationAccount2Id);

            ChannelAccount channelAccount1 = GetChannelAccount(conversationReference1, out bool isBot1);
            ChannelAccount channelAccount2 = GetChannelAccount(conversationReference2, out bool isBot2);

            bool channelAccountsMatch =
                (isBot1 == isBot2)
                && ((channelAccount1 == null && channelAccount2 == null)
                    || (channelAccount1 != null && channelAccount2 != null
                        && channelAccount1.Id.Equals(channelAccount2.Id)));

            return (conversationAccountsMatch && channelAccountsMatch);
        }

        /// <summary>
        /// Checks whether the given list of conversation references contains the given one.
        /// </summary>
        /// <param name="conversationReferences">The list of conversation references possibly containing the given one.</param>
        /// <param name="conversationReferenceToCheck">The conversation reference to check.</param>
        /// <returns>True, if the given conversation reference is contained by the list. False otherwise.</returns>
        public static bool Contains(
            IEnumerable<ConversationReference> conversationReferences,
            ConversationReference conversationReferenceToCheck)
        {
            if (conversationReferences != null)
            {
                foreach (ConversationReference conversationReference in conversationReferences)
                {
                    if (Match(conversationReference, conversationReferenceToCheck))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
