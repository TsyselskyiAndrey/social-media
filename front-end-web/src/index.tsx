import React from "react";
import ReactDOM from "react-dom/client";
import reportWebVitals from "./reportWebVitals";
import { IsAuthenticatedProvider } from "./Contexts/IsAuthenticatedContext";
import { GoogleOAuthProvider } from "@react-oauth/google";
import App from "./App";
import './index.css';

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement);
root.render(
  <React.StrictMode>
    <IsAuthenticatedProvider>
      <GoogleOAuthProvider clientId="470893192421-9jvk8phnp2bd1m2k6kd62hr59hj01rkk.apps.googleusercontent.com">
        <App />
      </GoogleOAuthProvider>
    </IsAuthenticatedProvider>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
