import { combineReducers } from 'redux';
import {reducer as FormReducer} from 'redux-form';
import eventReducer from './eventReducer';
import modalReducer from './modalReducer';
import authReducer from './authReducer';
import profileReducer from './profileReducer';
import asyncReducer from "../../../features/async/asyncReducer";
import {reducer as ToastrReducer} from 'react-redux-toastr';

const rootReducer = combineReducers({
  events: eventReducer,
  form: FormReducer,
  auth: authReducer,
  modals: modalReducer,
  async: asyncReducer,
  toastr: ToastrReducer,
  profile: profileReducer
});

export default rootReducer;
