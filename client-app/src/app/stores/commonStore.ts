import { ServerError } from "../models/ServerError";
import { makeAutoObservable, reaction } from 'mobx';

export default class CommonStore {
    error: ServerError | null = null;
    token: string | null = window.localStorage.getItem('jwt');
    appLoaded = false;

    constructor() {
        makeAutoObservable(this);
        
        //1st param: what we wanna react to. 2nd : can pass the token and do something
        reaction(
            () => this.token,
            token => {
                if (token) {
                    window.localStorage.setItem('jwt', token);
                } else {
                    window.localStorage.removeItem('jwt');
                }
            }
        )
    }

    setServerError = (error: ServerError) => {
        this.error = error;
    }

    setToken = (token: string | null) => {
        //dont need this line of code if we're doing it in reaction in the constructor
        //if (token) window.localStorage.setItem('jwt',token);
        this.token = token;
    }

    setAppLoaded = () => {
        this.appLoaded = true;
    }

}