import axios, { AxiosResponse } from 'axios';
import { toastr } from 'react-redux-toastr';

import { BrowserRouter as router} from "react-router-dom";

axios.defaults.baseURL = 'http://localhost:5000'; //process.env.REACT_APP_API_URL;

axios.interceptors.request.use(function (config) {
    const token = window.localStorage.getItem('jwt');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  }, function (error) {
    return Promise.reject(error);
  });

axios.interceptors.response.use(response => {
        return response.data;
    },error => {  
        if (error.message === 'Network Error' && !error.response) {
            toastr.error('Network error - make sure API is running!');
        }
        const { status, data, config, headers } = error.response;
        if (status === 404) {
            router.push('/notfound');
        }
        if (status === 401 && headers['www-authenticate'] === 'Bearer error="invalid_token", error_description="The token is expired"') {
            window.localStorage.removeItem('jwt');
            router.push('/')
            toastr.info('Your session has expired, please login again')
        }
        if (
            status === 400 &&
            config.method === 'get' &&
            data.errors.hasOwnProperty('id')
        ) {
            router.push('/notfound');
        }
        if (status === 500) {
            toastr.error('Server error - check the terminal for more info!');
        }
        throw error.response;
}); 
    
export const Api = {
    get,
    post,
    postForm,
    put,
    deleteDetail
};



function get(apiEndpoint){
    return axios.get(apiEndpoint)
}

function post(apiEndpoint, payload){
    return axios.post(apiEndpoint, payload, {
        headers: { 'Content-type': 'application/json;' }
      });
}
   
function postForm (apiEndpoint, file) {
    let formData = new FormData();
    formData.append('File', file);
    return axios.post(apiEndpoint, formData, {
        headers: { 'Content-type': 'multipart/form-data' }
      });
}

function put(apiEndpoint, payload){
    return axios.put(apiEndpoint, payload, getOptions()).then((response)=>{
        return response;
    }).catch((err)=>{
        console.log(err);
    })
}

function deleteDetail(apiEndpoint){
    return axios.delete(apiEndpoint, getOptions()).then((response)=>{
        return response;
    }).catch((err)=>{
        console.log(err);
    })
}

function getOptions(){
    let options = {}; 
    if(localStorage.getItem('token')){
        options.headers = { 'x-access-token': localStorage.getItem('token') };
        options.headers = { 'Content-type': 'application/json;' };
    }
    return options;
}