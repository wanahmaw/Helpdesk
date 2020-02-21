import React, { Component } from "react";
import * as H from "history";
import LoginForm from "./LoginForm";

interface IProps {
  location: H.Location;
  history: H.History;
}

class Login extends Component<IProps> {
  render() {
    return (
      <div className="container">
        <div className="row">
          <div className="col s12">
            <h3>Login</h3>
            {<LoginForm history={this.props.history} />}
          </div>
        </div>
      </div>
    );
  }
}

export default Login;
