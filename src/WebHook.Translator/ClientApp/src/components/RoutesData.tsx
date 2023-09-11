import React, { ReactNode } from "react";
import SignIn from "../pages/auth/SignIn";
import Home from "../pages/Home/Home";

interface IRoutes {
  path: string;
  component: ReactNode;
  exact?: boolean;
}

export const publicRoutes: IRoutes[] = [
  { path: "/", component: <SignIn /> },
  {
    path: "/forget-password",
    component: <div>forget password</div>,
  },
];

export const privateRoutes: IRoutes[] = [
  { path: "/dashboard", component: <Home /> },
];
