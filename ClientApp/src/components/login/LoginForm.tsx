import React from "react";
import { History } from "history";
import { useFormik } from "formik";
import * as Yup from "yup";
import { authentication } from "../../services/Authentication";

interface IProps {
  history: History;
}

const LoginForm = (props: IProps) => {
  // Pass the useFormik() HOC to setup the form
  const formik = useFormik({
    // Initial values
    initialValues: {
      username: "",
      password: ""
    },

    // Validate the form
    validationSchema: Yup.object({
      username: Yup.string()
        .max(50, "Must be 20 characters or less")
        .required("Required"),
      password: Yup.string()
        .max(1000, "Must be 20 characters or less")
        .required("Required")
    }),

    // Submit
    onSubmit: (values, actions) => {
      authentication.login(values.username, values.password).then(user => {
        console.log(user);
        if (user && user.token) {
          console.log("about to push");
          props.history.push("/dashboard");
        }
      });

      actions.setSubmitting(false);
      actions.resetForm({});
    }
  });

  // Explicit Formik form to blend with Materialize CSS
  return (
    <div className="row">
      <form className="col s12" onSubmit={formik.handleSubmit}>
        <div className="row">
          <div className="input-field col s12">
            <input
              id="username"
              type="text"
              {...formik.getFieldProps("username")}
            />
            <label htmlFor="username">Username</label>
            {formik.touched.username && formik.errors.username ? (
              <div>
                <span className="red-text text-darken-2">
                  {formik.errors.username}
                </span>
              </div>
            ) : null}
          </div>
        </div>
        <div className="row">
          <div className="input-field col s12">
            <input
              id="password"
              type="text"
              {...formik.getFieldProps("password")}
            ></input>
            <label htmlFor="password">Password</label>
            {formik.touched.password && formik.errors.password ? (
              <div>
                <span className="red-text text-darken-2">
                  {formik.errors.password}
                </span>
              </div>
            ) : null}
          </div>
        </div>
        <br />
        <button
          type="reset"
          className="btn waves-effect waves-light red"
          onClick={() => formik.resetForm()}
          style={{ marginRight: "10px" }}
        >
          Clear
          <i className="material-icons right">cancel</i>
        </button>
        <button type="submit" className="btn waves-effect waves-light blue">
          Submit
          <i className="material-icons right">send</i>
        </button>
      </form>
    </div>
  );
};

export default LoginForm;
