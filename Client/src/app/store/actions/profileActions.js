import { toastr } from 'react-redux-toastr';
import cuid from 'cuid';
import { Api } from "../../apis/Index";
import {  asyncActionStart,  asyncActionFinish,  asyncActionError} from "../../../features/async/asyncActions";
import { LOGIN_USER, UPLOAD_PHOTO, LOAD_PROFILE} from "./actionTypes";
import { setUserDetails} from "./authActions";


export const loadProfile = username => async dispatch => {
    const profile = await Api.get(`/api/profiles/${username}`);
    dispatch({ type: LOAD_PROFILE, payload: {profile} });
};

export const uploadProfileImage = (file, fileName) => async (dispatch, getState) => {
    
    try {
      dispatch(asyncActionStart());
      console.log("adsad")

      const user = getState().auth.currentUser;
      console.log(user)
      const profile =  await Api.get(`/api/profiles/${user.Username}`)
      
      console.log("profile ")
      console.log(profile)

      const photo = await Api.postForm("/api/photos", file)
      
      console.log(photo)
      //dispatch(setUserDetails(user));
      
      if (photo.IsMain && user) {

        console.log("photo ")
        user.Image = photo.Url;
        profile.Image = photo.Url;
        profile.Photos.push(photo);
      

       dispatch({ type: LOGIN_USER, payload: {user }});
       dispatch({ type: UPLOAD_PHOTO, payload: {profile} });
      }
      dispatch(asyncActionFinish());
    } catch (error) {
      console.log(error);
      dispatch(asyncActionError());
    }
  };


export const deletePhoto = () => {
    return null;
}

  
export const setMainPhoto = () => {
    return null;
}