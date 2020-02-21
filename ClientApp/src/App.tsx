import React, { Component } from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { FetchData } from "./components/FetchData";
import { Counter } from "./components/Counter";
import ProtectedRoute from "./components/ProtectedRoute";
import Login from "./components/login/Login";

import "./custom.css";

class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <BrowserRouter>
        <Layout>
          <Route exact path="/" component={Login} />
          <ProtectedRoute path="/counter" component={Counter} />
          <ProtectedRoute path="/fetch-data" component={FetchData} />
        </Layout>
      </BrowserRouter>
    );
  }
}

export default App;
