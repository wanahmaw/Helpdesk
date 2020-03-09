import React, { Component } from "react";
import AddTicketForm from "./AddTicketForm";
import { History, Location } from "history";

// TODO: Insert owner id into AddTicketForm

interface IProps {
  location: Location;
  history: History;
}

class AddTicket extends Component<IProps, any> {
  componentDidMount() {
    console.log("AddTicket mounted");
  }
  render() {
    return (
      <div className="container">
        <div className="row">
          <div className="col s12">
            <h4>Add Ticket</h4>
            {<AddTicketForm history={this.props.history} />}
          </div>
        </div>
      </div>
    );
  }
}

export default AddTicket;
