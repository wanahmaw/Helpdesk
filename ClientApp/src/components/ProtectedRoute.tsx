import React from "react";
import {
  Route,
  RouteComponentProps,
  RouteProps,
  Redirect
} from "react-router-dom";
import { authentication } from "../services/Authentication";

export interface ProtectedRouteProps extends RouteProps {
  component:
    | React.ComponentType<RouteComponentProps<any>>
    | React.ComponentType<any>;
}

// Citation: https://stackoverflow.com/questions/47747754/how-to-rewrite-the-protected-router-using-typescript-and-react-router-4
const ProtectedRoute = ({ component, ...rest }: ProtectedRouteProps) => {
  const user = authentication.getCurrentUser();
  const routeComponent = (props: any) =>
    // CHECK USER HERE
    user ? (
      React.createElement(component, props)
    ) : (
      <Redirect to={{ pathname: "/", state: { from: props.location } }} />
    );
  // RETURN COMPONENT SINCE IT'S AUTHORIZED
  return <Route {...rest} component={routeComponent} />;
};

export default ProtectedRoute;
