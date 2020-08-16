import { LOGIN_USER, SIGN_OUT_USER} from "./actionTypes";
import { SubmissionError } from 'redux-form';
import { closeModal } from './modalActions';
import { Api } from "../../apis/Index";
import {  asyncActionStart,  asyncActionFinish,  asyncActionError} from "../../../features/async/asyncActions";
  
export const login = cred => async (dispatch) => {
    try {
      dispatch(asyncActionStart());

      const data = {
        "email": "bob@test.com",
        "password": "Pa$$w0rd"
      };

      const user =  await Api.post("/api/user/login", data)
  
      dispatch(setToken(user))     
      dispatch(closeModal())
      dispatch(asyncActionFinish());
      return user;
  
    } catch (error) {
      dispatch(asyncActionError())
      throw new SubmissionError({ 
        _error: error
      });
    }
  };
  
  export const registerUser = userValues => async (dispatch) => {  
    try {

      dispatch(asyncActionStart());

      const user = await Api.post("/api/user/register", userValues)

      dispatch(setToken(user))
      dispatch(closeModal());
      dispatch(asyncActionFinish());
     
    } catch (error) {
      dispatch(asyncActionError())
      throw new SubmissionError({
        _error: error
      });
     
    }
};

export const socialLogin = (response) => async (dispatch) => {   
    try {

      dispatch(asyncActionStart());
      let accessToken = response.accessToken;
      const user = await Api.post("api/user/facebook", {accessToken})
     
      dispatch(setToken(user))
      dispatch(closeModal());
      dispatch(asyncActionFinish());
     
      } catch (error) {
        console.log(error);      
        dispatch(asyncActionError())      
        
     
    }
}

export const logout = () => {
  return dispatch => {
    window.localStorage.removeItem('jwt');
    dispatch({  type: SIGN_OUT_USER});
  };  
}

export const setToken = (user) => {
  return dispatch => {
    window.localStorage.setItem('jwt', user.Token);
    dispatch({ type: LOGIN_USER, payload: {user }});
  };
}

export const getUser = () => async (dispatch) => {  
  try {

    dispatch(asyncActionStart());

    const user = await Api.post("/api/user")
   
    dispatch(asyncActionFinish());
   
  } catch (error) {
    dispatch(asyncActionError())
    throw new SubmissionError({
      _error: error
    });
   
  }
};


export function setUserDetails(user){
  return dispatch => {
    dispatch({ type: LOGIN_USER, payload: {user }});
  };
}

export const updatePassword = () => {
  return null;
}
