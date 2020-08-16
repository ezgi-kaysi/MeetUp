import { Api } from "../../apis/Index";
import { FETCH_EVENTS, FETCH_EVENT, CREATE_EVENT, UPDATE_EVENT, DELETE_EVENT } from "./actionTypes";
import {  asyncActionStart,  asyncActionFinish,  asyncActionError} from "../../../features/async/asyncActions";
import { toastr } from 'react-redux-toastr';

import { fetchSampleData } from "../../data/mockApi";


export const loadEvents = () => {
  return async dispatch => {
      try {
          dispatch(asyncActionStart())
          const events = await fetchSampleData();
          dispatch({type: FETCH_EVENTS, payload: {events}})
          dispatch(asyncActionFinish())
      } catch (error) {
          console.log(error);
          dispatch(asyncActionError())
      }
  }
}

export const getEventsForDashboard = lastEvent => async (
  dispatch
) => {
  try {
    dispatch(asyncActionStart());

    let activitiesEnvelope = await Api.get("/api/activities")

    const { activities, activityCount } = activitiesEnvelope;

    let events = [];

    for (let i = 0; i < activities.length; i++) {
      let evt = { ...activities[i], id: activities[i].id };
      events.push(evt);
    }

    dispatch({ type: FETCH_EVENTS, payload: { events } });
    dispatch(asyncActionFinish());
    return activitiesEnvelope;

  } catch (error) {
    console.log(error);
    dispatch(asyncActionError());
  }
};


export const fetchEvent = id => async dispatch => {
  const event = await Api.get(`/api/activities/${id}`);
  dispatch({ type: FETCH_EVENT, payload: {event} });
};

export const createEvent = event => {
  return async dispatch => {
    try {
      dispatch({
        type: CREATE_EVENT,
        payload: {
          event
        }
      });
      toastr.success('Success!', 'Event has been created');
    } catch (error) {
      toastr.error('Oops', 'Something went wrong');
    }
  };
};

export const updateEvent = event => {
  return async dispatch => {
    try {
      dispatch({
        type: UPDATE_EVENT,
        payload: {
          event
        }
      });
      toastr.success('Success!', 'Event has been updated');
    } catch (error) {
      toastr.error('Oops', 'Something went wrong');
    }
  };
};

export const deleteEvent = eventId => {
  return {
    type: DELETE_EVENT,
    payload: {
      eventId
    }
  };
};