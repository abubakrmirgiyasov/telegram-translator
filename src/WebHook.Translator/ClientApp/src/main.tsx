import React from "react";
import ReactDOM from "react-dom/client";
import App from "./components/App/App.tsx";
import "./assets/scss/index.scss";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import configureStore from "./store/index";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <Provider store={configureStore}>
    <React.StrictMode>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </React.StrictMode>
  </Provider>
);
