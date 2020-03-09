import React from "react";
import { History } from "history";
import { useFormik } from "formik";
import * as Yup from "yup";
import { ticketing } from "../../services/Ticketing";

interface IProps {
  history: History;
  ticket: any;
}

const UpdateTicketForm = (props: IProps) => {
  // Pass the useFormik() HOC to setup the form
  const formik = useFormik({
    // Initial values
    initialValues: {
      title: props.ticket.title,
      content: props.ticket.content
    },

    // Validate the form
    validationSchema: Yup.object({
      title: Yup.string()
        .max(50, "Must be 50 characters or less")
        .required("Required"),
      content: Yup.string()
        .max(1000, "Must be 1000 characters or less")
        .required("Required")
    }),

    // Submit
    onSubmit: (values, actions) => {
      console.log("Updating ticket # " + props.ticket.ticketId + " ->", values);
      const ticketId = props.ticket.ticketId;
      // Update ticket
      ticketing.updateTicket(values, ticketId, props.history);
      // Finalize form after submitting
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
            <input id="title" type="text" {...formik.getFieldProps("title")} />
            <label className="active" htmlFor="title">
              Title
            </label>
            {formik.touched.title && formik.errors.title ? (
              <div>
                <span className="red-text text-darken-2">
                  {formik.errors.title}
                </span>
              </div>
            ) : null}
          </div>
        </div>
        <div className="row">
          <div className="input-field col s12">
            <textarea
              id="content"
              className="materialize-textarea"
              {...formik.getFieldProps("content")}
            ></textarea>
            <label className="active" htmlFor="title">
              Content
            </label>
            {formik.touched.content && formik.errors.content ? (
              <div>
                <span className="red-text text-darken-2">
                  {formik.errors.content}
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

export default UpdateTicketForm;
