import { useAxios } from "../../hooks/useAxios";
import { ISignUp } from "../../types/auth/ISignUp";
import { Constants, RequestType } from "../../common/Constants";
import { ISignIn } from "../../types/auth/ISignIn";
import { IUser } from "../../types/user/IUser";
import axios from "axios";

export const register = (values: ISignUp) => {
  const options = {
    url: "",
    request: RequestType.post,
  };

  const response = useAxios<ISignUp>(options, values);

  if (response.loading) if (response.data) return response.data;
  return response.error;
};

export const login = (values: ISignIn) => {
  axios
    .post("")
    .then((response) => {
      const data: IUser = response.data;
      if (data.accessToken)
        localStorage.setItem(Constants.USER_STORAGE, JSON.stringify(data));
      return data;
    })
    .catch((error) => {
      throw error;
    });
};

export const logout = () => {
  const options = {
    url: "",
    request: RequestType.delete,
  };

  const response = useAxios<string>(options);
};

const refreshToken = (values: IUser) => {
  const options = {
    url: "",
    request: RequestType.put,
  };

  const response = useAxios<IUser>(options, values);
};
