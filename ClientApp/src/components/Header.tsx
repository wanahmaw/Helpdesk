import React, { Component } from "react";
import { Link } from "react-router-dom";
import { authentication } from "../services/Authentication";

interface State {
  user: any;
}

class Header extends Component<{}, State> {
  constructor(props: any) {
    super(props);

    this.state = {
      user: {}
    };
  }

  componentDidMount() {
    const user = authentication.getCurrentUser();
    this.setState({
      user
    });
  }

  // LOGOUT
  // 1. Remove access token & redirect
  renderHeaderContent() {
    const user = this.state.user;
    if (user) {
      return (
        <div>
          <li key="1" style={{ fontSize: "large" }}>
            {user.userName}
          </li>
          <li key="2">
            <a
              style={{ fontSize: "large", fontWeight: "bold" }}
              href="/#"
              onClick={authentication.logout}
            >
              Log Out
            </a>
          </li>
        </div>
      );
    } else {
      return (
        <div>
          <a href="/">Log In</a>
        </div>
      );
    }
  }

  render() {
    return (
      <nav>
        <div className="nav-wrapper blue accent-2">
          <Link to="/dashboard" className="brand-logo left">
            <i className="material-icons">help_outline</i>
            HelpDesk
          </Link>
          <ul className="right">{this.renderHeaderContent()}</ul>
        </div>
      </nav>
    );
  }
}

export default Header;
