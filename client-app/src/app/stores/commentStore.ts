import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { ChatComment } from './../models/comment';
import { makeAutoObservable, runInAction } from 'mobx';
import { store } from './store';

export default class CommentStore {

    comments: ChatComment[] = [];
    hubConnection : HubConnection | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    creatHubConnection = (activityId : string) => {
        if(store.activityStore.selectedActivity) {
            this.hubConnection = new HubConnectionBuilder()
                .withUrl(process.env.REACT_APP_CHAT_URL + '?activityid=' + activityId, {
                    accessTokenFactory: () => store.userStore.user?.token!
                })
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            this.hubConnection.start().catch(error => console.log("Error establishing the connection", error));

            this.hubConnection.on('LoadComments', (comments : ChatComment[]) => {
                runInAction(() => {
                    comments.forEach(comment => {
                        comment.createdAt = new Date(comment.createdAt+ 'Z');
                    })
                    this.comments = comments
                });
            });

            this.hubConnection.on('ReceiveComment', (comment : ChatComment) => {
                runInAction(() => {
                    comment.createdAt = new Date(comment.createdAt);
                    this.comments.unshift(comment) //places new comments to the start of the array
                });
            });
        }
    }
    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error => console.log('Error stopping Connection', error));
    }

    clearComments = () => {
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (values: any) => {
        values.activityId = store.activityStore.selectedActivity?.id;
        try {//name of methods should match on client and SignalR
            await this.hubConnection?.invoke('SendComment', values);
        } catch (error) {
            console.log(error);
        }
    }
}

