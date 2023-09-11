import { IUser } from "../types/user/IUser";
import { Constants } from "../common/Constants";
import { useEffect, useState } from "react";

interface IUseProfile {
  isLoading: boolean;
  user: IUser | null;
  token: string | null;
}

const isLoggedInUser: () => IUser | null = () => {
  const json = localStorage.getItem(Constants.USER_STORAGE);
  return json ? JSON.parse<IUser>(json) : null;
};

export const useProfile = (): IUseProfile => {
  const userProfileSession = isLoggedInUser();
  const token = userProfileSession && userProfileSession["accessToken"];

  const [isLoading, setIsLoading] = useState<boolean>(!!userProfileSession);
  const [user, setUser] = useState<IUser | null>(userProfileSession);
  useEffect(() => {
    const userProfileSession = isLoggedInUser();
    const token = userProfileSession && userProfileSession["accessToken"];
    setUser(userProfileSession);
    setIsLoading(!!token);
  }, []);

  return { user, isLoading, token };
};
