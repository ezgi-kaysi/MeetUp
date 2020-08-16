import { createReducer } from "../../common/util/reducerUtils";
import { LOGIN_USER, SIGN_OUT_USER } from "../../../app/store/actions/actionTypes";

const initialState = {
    authenticated: false,
    currentUser: null
}

const loginUser = (state, payload) => {
    return {
        authenticated: true,
        currentUser: payload.user
    }
}

const signOutUser = () => {
    return {
        authenticated: false,
        currentUser: null
    }
}

export default createReducer(initialState, {
    [LOGIN_USER]: loginUser,
    [SIGN_OUT_USER]: signOutUser
}) 