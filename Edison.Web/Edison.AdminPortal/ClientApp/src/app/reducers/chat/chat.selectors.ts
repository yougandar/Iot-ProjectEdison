import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AppState } from '..';
import { State, selectAll } from './chat.reducer';

const onlyUnique = (value, index, self) => {
    return self.indexOf(value.id) === index;
}

export const chatStateSelector = createFeatureSelector<AppState, State>('chat');

export const chatAuthSelector = createSelector(chatStateSelector, state => ({
    token: state.token,
    user: state.userContext
}));

export const chatMessagesSelector = createSelector(chatStateSelector,
    state => selectAll(state)
        .filter(chat => chat.channelData &&
            chat.channelData.data &&
            chat.channelData.data.from));

export const chatActiveUserIdSelector = createSelector(chatStateSelector, state => state.activeUserId);

export const chatActiveUserSelector = createSelector(chatStateSelector, state => ({ name: state.activeUserName, userId: state.activeUserId }))

export const chatShowUserChatSelector = createSelector(chatActiveUserIdSelector, userId => userId && userId !== '*')

export const chatShowAllChatSelector = createSelector(chatActiveUserIdSelector, userId => userId === '*')

export const chatActiveConversationIdSelector = createSelector(chatStateSelector, state => state.activeConversationId);

export const chatActiveUsersSelector = createSelector(
    chatMessagesSelector,
    messages => {
        const users = messages
            .filter(chat => chat.channelData.data.from.role.toLowerCase() === 'consumer')
            .map(chat => ({
                id: chat.channelData.data.from.id,
                conversationId: chat.conversation.id,
                name: chat.channelData.data.from.name,
            }))
            .filter((chat, index, self) => self.findIndex(c => c.id === chat.id) === index)

        return users;
    });

export const chatActiveUsersCountSelector = createSelector(
    chatActiveUsersSelector,
    users => users.length,
)

export const chatAllMessagesSelector = createSelector(
    chatMessagesSelector,
    messages => messages
        .filter(m => m.channelData.data.from.role.toLowerCase() === 'admin' && m.channelData.data.userId === '*' ||
            m.channelData.data.from.role.toLowerCase() !== 'admin')
        .map(m => ({
            name: m.channelData.data.from.role.toLowerCase() === 'admin' ? 'YOU' : m.channelData.data.from.name,
            text: m.text,
            id: m.id,
        }))
)

export const chatActiveMessagesSelector = createSelector(
    chatMessagesSelector,
    chatActiveUserIdSelector,
    (messages, activeUserId) => {
        if (activeUserId) {
            const firstUserMessage = messages.find(m => m.channelData.data.userId === activeUserId);
            return messages
                .filter(m => m.channelData.data.userId === activeUserId ||
                    (m.channelData.data.userId === '*' && new Date(m.timestamp) > new Date(firstUserMessage.timestamp)))
                .map(m => ({
                    name: m.channelData.data.from.role.toLowerCase() === 'admin' ? 'YOU' : m.channelData.data.from.name,
                    text: m.text,
                    id: m.id,
                }))
        }

        return [];
    }
)
