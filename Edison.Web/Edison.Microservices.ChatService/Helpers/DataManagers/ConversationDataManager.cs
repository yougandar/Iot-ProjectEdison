using Edison.Core.Common.Models;
using System.Threading.Tasks;
using AutoMapper;
using System;
using Edison.Common.Interfaces;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using System.Net;
using Edison.ChatService.Models.DAO;
using Edison.ChatService.Repositories;

namespace Edison.ChatService.Helpers
{
    public class ConversationDataManager
    {
        private ICosmosDBRepositoryChat<ConversationDAO> _repoConversations;

        public ConversationDataManager(ICosmosDBRepositoryChat<ConversationDAO> repoDevices)
        {
            _repoConversations = repoDevices;
        }

        public async Task<ConversationModel> GetConversationById(string conversationId)
        {
            ConversationDAO conversationEntity = await _repoConversations.GetItemAsync(conversationId);
            return Mapper.Map<ConversationModel>(conversationEntity);
        }

        public async Task<ConversationModel> GetActiveConversationFromUser(string userId)
        {
            ConversationDAO conversationEntity = await _repoConversations.GetItemAsync(p => p.UserId == userId.ToLower() && p.EndDate.Value == null);
            return Mapper.Map<ConversationModel>(conversationEntity);
        }

        public async Task<IEnumerable<ConversationModel>> GetConversationsFromUser(string userId)
        {
            IEnumerable<ConversationDAO> conversationEntities = await _repoConversations.GetItemsAsync(p => p.UserId == userId.ToLower());
            return Mapper.Map<IEnumerable<ConversationModel>>(conversationEntities);
        }

        public async Task<IEnumerable<ConversationModel>> GetActiveConversations()
        {
            IEnumerable<ConversationDAO> conversationEntities = await _repoConversations.GetItemsAsync(p => p.EndDate.Value == null);
            return Mapper.Map<IEnumerable<ConversationModel>>(conversationEntities);
        }

        public async Task<ConversationModel> CreateOrUpdateConversation(ConversationLogCreationModel conversationLogObj)
        {
            if (string.IsNullOrEmpty(conversationLogObj.UserId))
                throw new Exception($"No userId found.");

            ConversationDAO conversationDAO = await _repoConversations.GetItemAsync(p => p.UserId == conversationLogObj.UserId.ToLower() && p.EndDate.Value == null);

            //Create
            if (conversationDAO == null)
                return await CreateConversation(conversationLogObj);

            //Update
            ConversationLogDAOObject conversationLogDAO = Mapper.Map<ConversationLogDAOObject>(conversationLogObj.Message);
            conversationDAO.ConversationLogs.Add(conversationLogDAO);
            if (conversationLogObj.ReportType != ChatReportType.Unknown)
                conversationDAO.ReportType = conversationLogObj.ReportType.ToString();
            if(!string.IsNullOrWhiteSpace(conversationLogObj.Username))
                conversationDAO.Username = conversationLogObj.Username;

            try
            {
                await _repoConversations.UpdateItemAsync(conversationDAO);
            }
            catch (DocumentClientException e)
            {
                //Update concurrency issue, retrying
                if (e.StatusCode == HttpStatusCode.PreconditionFailed)
                    return await CreateOrUpdateConversation(conversationLogObj);
                throw e;
            }

            return Mapper.Map<ConversationModel>(conversationDAO);
        }

        public async Task<ConversationModel> CreateConversation(ConversationLogCreationModel conversationLogObj)
        {
            ConversationLogDAOObject conversationLogDAO = Mapper.Map<ConversationLogDAOObject>(conversationLogObj.Message);
            ConversationDAO newConversationDAO = new ConversationDAO()
            {
                ConversationLogs = new List<ConversationLogDAOObject>() { conversationLogDAO },
                ReportType = conversationLogObj.ReportType.ToString(),
                UserId = conversationLogObj.UserId.ToLower(),
                Username = conversationLogObj.Username
            };
            newConversationDAO.Id = await _repoConversations.CreateItemAsync(newConversationDAO);
            if (string.IsNullOrEmpty(newConversationDAO.Id))
                throw new Exception($"An error occured when creating a new conversation: {conversationLogObj.UserId}");

            return Mapper.Map<ConversationModel>(newConversationDAO);
        }

        public async Task<ConversationModel> CloseConversation(ConversationLogCloseModel conversationCloseObj)
        {
            if (string.IsNullOrEmpty(conversationCloseObj.UserId))
                throw new Exception($"No userId found.");

            ConversationDAO conversationDAO = await _repoConversations.GetItemAsync(p => p.UserId == conversationCloseObj.UserId.ToLower() && p.EndDate.Value == null);

            //If already closed, we do nothing
            if (conversationDAO == null)
                return null;

            conversationDAO.EndDate = conversationCloseObj.EndDate;

            try
            {
                await _repoConversations.UpdateItemAsync(conversationDAO);
            }
            catch (DocumentClientException e)
            {
                //Update concurrency issue, retrying
                if (e.StatusCode == HttpStatusCode.PreconditionFailed)
                    await CloseConversation(conversationCloseObj);
                throw e;
            }

            return Mapper.Map<ConversationModel>(conversationDAO);
        }
    }
}
