import axios from 'axios';
import appconfig from '../config';

const api = axios.create({
    baseURL: appconfig.baseURL,
    headers: {
        'Content-Type': 'application/json',
    },
});
api.defaults.withCredentials = true;

const GameService = {
    startGame: () => api.get(`/start`)
        .then(response => {
            return response;
        })
        .catch(error => {
            console.error("Error starting game:", error);
        }),

    getStatus: () => api.get(`/status`)
        .then(response => {
            return response;
        })
        .catch(error => {
            console.error("Error fetching game status:", error);
        }),

    attack: (x, y) => api.post(`/attack`, { x, y })
        .then(response => {
            return response;
        })
        .catch(error => {
            console.error("Error during attack:", error);
        })
};

export default GameService;
