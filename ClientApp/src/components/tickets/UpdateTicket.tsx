import React, { Component } from "react";
import UpdateTicketForm from "./UpdateTicketForm";
import { History, Location } from "history";

interface IProps {
  history: History;
  location: Location;
}

class UpdateTicket extends Component<IProps> {
  render() {
    const ticket = this.props.location.state.ticket;
    return (
      <div className="container">
        <div className="row">
          <div className="col s12">
            <h4>Update Ticket</h4>
            {<UpdateTicketForm history={this.props.history} ticket={ticket} />}
          </div>
        </div>
      </div>
    );
  }
}

export default UpdateTicket;
