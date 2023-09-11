import React, { FormEvent, useEffect, useState } from "react";
import { Routes, Route } from "react-router-dom";
import axios from "axios";
import { privateRoutes, publicRoutes } from "../RoutesData";

function App() {
  return (
    <React.Fragment>
      <Routes>
        <Route>
          {privateRoutes.map((route, key) => (
            <Route
              key={key}
              path={route.path}
              element={route.component}
              exact={true}
            />
          ))}
        </Route>
        <Route>
          {publicRoutes.map((route, key) => (
            <Route
              key={key}
              path={route.path}
              element={route.component}
              exact={true}
            />
          ))}
        </Route>
      </Routes>
    </React.Fragment>
  );
}

const Error = () => {
  return <h2>Error page</h2>;
};

const Home = () => {
  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const data = {
      question: e.currentTarget.question.value,
      options: [e.currentTarget.option1.value, e.currentTarget.option2.value],
      correctOption: e.currentTarget.correctOption.value,
      hint: e.currentTarget.hint.value,
    };

    axios
      .post("/api/admin/createquestion", data)
      .then((r) => console.log(r))
      .catch((e) => console.log(e));
  };

  return (
    <div>
      <div>
        <form
          style={{ display: "flex", flexDirection: "column" }}
          onSubmit={handleSubmit}
        >
          <input type={"text"} name={"question"} placeholder={"text"} />
          <input type={"text"} name={"option1"} placeholder={"option1"} />
          <input type={"text"} name={"option2"} placeholder={"option1"} />
          <input type={"text"} name={"hint"} placeholder={"hint"} />
          <input type={"text"} name={"correctOption"} placeholder={"correct"} />
          <input type={"submit"} />
        </form>
      </div>
    </div>
  );
};

const Test = () => {
  return <h1>Foo Page</h1>;
};

const About = () => {
  return <h1>Bar Page</h1>;
};

export default App;
