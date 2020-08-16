import { createReducer } from "../../common/util/reducerUtils";
import {
  UPLOAD_PHOTO,
  SETMAIN_PHOTO,
  DELETE_PHOTO,
  LOAD_PROFILE
} from "../../../app/store/actions/actionTypes";

const initialState = {
  profile: {
    displayName: "",
    username: "",
    bio: "",
    image: "",
    following: "",
    followersCount: 0,
    followingCount: 0,
    photos: [],
  }
};

const loadProfile = (state, payload) => {
    return {
      profile: payload
    };
  };


const uploadPhoto = (state, payload) => {
  return {
    profile: payload
  };
};

const setMainPhoto = (state, payload) => {
  return {};
};

const deletePhoto = (state, payload) => {
  return {};
};

export default createReducer(initialState, {
  [UPLOAD_PHOTO]: uploadPhoto,
  [SETMAIN_PHOTO]: setMainPhoto,
  [DELETE_PHOTO]: deletePhoto,
  [LOAD_PROFILE]: loadProfile,
  // [FETCH_EVENT]: follow,
  // [FETCH_EVENT]: unfollow,
  // [FETCH_EVENT]: listFollowings,
  // [FETCH_EVENT]: listActivities
});
