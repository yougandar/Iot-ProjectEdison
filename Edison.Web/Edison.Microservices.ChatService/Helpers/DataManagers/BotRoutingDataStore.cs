using AutoMapper;
using Edison.ChatService.Helpers.Interfaces;
using Edison.ChatService.Models.DAO;
using Edison.ChatService.Repositories;
using Edison.Core.Common.Models;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edison.ChatService.Helpers
{
    /// <summary>
    /// Routing data store that stores the data in Azure Table Storage.
    /// See the IRoutingDataStore interface for the general documentation of properties and methods.
    /// </summary>
    [Serializable]
    public class BotRoutingDataStore : IRoutingDataStore
    {
        private ICosmosDBRepositoryChat<ConversationReferenceDAO> _repoConversationReferencesUsers;

        public BotRoutingDataStore(ICosmosDBRepositoryChat<ConversationReferenceDAO> repoConversationReferencesUsers)
        {
            _repoConversationReferencesUsers = repoConversationReferencesUsers;
        }

        #region Get region

        public async Task<IEnumerable<ConversationReference>> GetUsers()
        {
            var result = await _repoConversationReferencesUsers.GetItemsAsync(p => p.DataType == ChatDataType.ConversationReference);
            return Mapper.Map<IEnumerable<ConversationReference>>(result);
        }

        public async Task<IEnumerable<ConversationReference>> GetConversationsFromUser(string userId)
        {
            var result = await _repoConversationReferencesUsers.GetItemsAsync(p => p.DataType == ChatDataType.ConversationReference && p.Id == userId);
            return Mapper.Map<IEnumerable<ConversationReference>>(result);
        }

        public async Task<IEnumerable<ConversationReference>> GetConversations(ChatUserRole chatUserRole)
        {
            var result = await _repoConversationReferencesUsers.GetItemsAsync(p => p.Role == chatUserRole.ToString());
            return Mapper.Map<IEnumerable<ConversationReference>>(result);
        }

        public async Task<IEnumerable<ConversationReference>> GetConversations()
        {
            var result = await _repoConversationReferencesUsers.GetItemsAsync();
            return Mapper.Map<IEnumerable<ConversationReference>>(result);
        }

        /*public IList<ConversationReference> GetAggregationChannels()
        {
            var entities = GetAllEntitiesFromTable(_aggregationChannelsTable).Result;
            return GetAllConversationReferencesFromEntities(entities);
        }

        public IList<ConnectionRequest> GetConnectionRequests()
        {
            var entities = GetAllEntitiesFromTable(_connectionRequestsTable).Result;
            var connectionRequests = new List<ConnectionRequest>();

            foreach (RoutingDataEntity entity in entities)
            {
                var connectionRequest =
                    JsonConvert.DeserializeObject<ConnectionRequest>(entity.Body);
                connectionRequests.Add(connectionRequest);
            }

            return connectionRequests;
        }

        public IList<Connection> GetConnections()
        {
            var entities = GetAllEntitiesFromTable(_connectionsTable).Result;
            var connections = new List<Connection>();

            foreach (RoutingDataEntity entity in entities)
            {
                var connection =
                    JsonConvert.DeserializeObject<Connection>(entity.Body);
                connections.Add(connection);
            }

            return connections;
        }*/

        #endregion

        #region Add region

        public async Task<bool> AddConversationReference(ConversationReference conversationReference)
        {
            ConversationReferenceDAO refDAO = Mapper.Map<ConversationReferenceDAO>(conversationReference);
            refDAO.DataType = ChatDataType.ConversationReference;

            string id = await _repoConversationReferencesUsers.CreateOrUpdateItemAsync(refDAO);
            return !string.IsNullOrEmpty(id);
        }

        /*public bool AddAggregationChannel(ConversationReference aggregationChannel)
        {
            string rowKey = aggregationChannel.Conversation.Id;
            string body = JsonConvert.SerializeObject(aggregationChannel);

            return InsertEntityToTable(rowKey, body, _aggregationChannelsTable);
        }

        public bool AddConnectionRequest(ConnectionRequest connectionRequest)
        {
            string rowKey = connectionRequest.Requestor.Conversation.Id;
            string body = JsonConvert.SerializeObject(connectionRequest);

            return InsertEntityToTable(rowKey, body, _connectionRequestsTable);
        }

        public bool AddConnection(Connection connection)
        {
            string rowKey = connection.ConversationReference1.Conversation.Id +
                connection.ConversationReference2.Conversation.Id;
            string body = JsonConvert.SerializeObject(connection);

            return InsertEntityToTable(rowKey, body, _connectionsTable);
        }*/

        #endregion

        #region Remove region

        /*public async Task<bool> RemoveConversationReference(ConversationReference conversationReference)
        {
            if (conversationReference.Bot != null)
            {
                return await _repoConversationReferences.DeleteItemsAsync(p => p.Id == conversationReference.Conversation.Id && p.Bot != null);
            }
            else
            {
                return await _repoConversationReferences.DeleteItemsAsync(p => p.Id == conversationReference.Conversation.Id && p.Bot == null);
            }
        }*/

        /*public bool RemoveAggregationChannel(ConversationReference aggregationChannel)
        {
            string rowKey = aggregationChannel.Conversation.Id;
            return AzureStorageHelper.DeleteEntryAsync<RoutingDataEntity>(
                _aggregationChannelsTable, DefaultPartitionKey, rowKey).Result;
        }

        public bool RemoveConnectionRequest(ConnectionRequest connectionRequest)
        {
            string rowKey = connectionRequest.Requestor.Conversation.Id;
            return AzureStorageHelper.DeleteEntryAsync<RoutingDataEntity>(
                _connectionRequestsTable, DefaultPartitionKey, rowKey).Result;
        }

        public bool RemoveConnection(Connection connection)
        {
            string rowKey = connection.ConversationReference1.Conversation.Id +
                connection.ConversationReference2.Conversation.Id;
            return AzureStorageHelper.DeleteEntryAsync<RoutingDataEntity>(
                _connectionsTable, DefaultPartitionKey, rowKey).Result;
        }*/

        #endregion
    }
}
