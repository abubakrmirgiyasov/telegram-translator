import {IUser} from "../../types/user/IUser";
import {Constants} from "../../common/Constants";
import {LOGIN_FAIL, LOGIN_SUCCESS, LOGOUT, REGISTER_FAIL, REGISTER_SUCCESS} from "./actionType";

interface IAuth {
  isLoggedIn: boolean;
  user?: IUser | null;
}

const user: IUser = JSON.parse(localStorage.getItem(Constants.USER_STORAGE));

const initState: IAuth = user
    ? {isLoggedIn: true, user: user}
    : {isLoggedIn: false, user: null};

export default function (state = initState, action) {
  const {type, payload}: { type: string; payload: IAuth } = action;

  switch (type) {
    case REGISTER_SUCCESS:
      return {
        ...state,
        isLoggedIn: false,
        user: null,
      };
    case REGISTER_FAIL:
      return {
        ...state,
        isLoggedIn: false,
        user: null,
      };
    case LOGIN_SUCCESS:
      return {
        ...state,
        isLoggedIn: true,
        user: payload.user,
      };
    case LOGIN_FAIL:
      return {
        ...state,
        isLoggedIn: false,
        user: null,
      };
    case LOGOUT:
      return {
        ...state,
        isLoggedIn: false,
        user: null,
      };
    default:
      return state;
  }
}
