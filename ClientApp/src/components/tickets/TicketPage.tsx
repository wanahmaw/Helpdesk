import React, { Component } from "react";
import * as H from "history";
import { Link, RouteComponentProps } from "react-router-dom";
import { authentication } from "../../services/Authentication";
import { ticketing } from "../../services/Ticketing";

interface Props extends RouteComponentProps {
  location: H.Location;
  history: H.History;
  ticket?: any;
}

class TicketPage extends Component<Props> {
  renderUpdateLink(ticket: any) {
    // Get user role
    const roleId = authentication.getUserRole();
    if (roleId == 1 || 2) {
      return (
        <Link
          to={{
            pathname: `/ticket-update/${ticket.ticketId}`,
            state: {
              ticket: ticket
            }
          }}
        >
          <span className="blue-text text-darken-3">Update</span>
        </Link>
      );
    }
  }

  renderDeleteButton(ticket: any) {
    // Get user role
    const roleId = authentication.getUserRole();
    if (roleId == 2) {
      return (
        <button
          style={{
            padding: "0",
            border: "none",
            background: "none"
          }}
          type="button"
          className="material-icons"
          onClick={e => {
            e.preventDefault();
            ticketing
              .deleteTicket(ticket.ticketId)
              .then(() => {
                this.props.history.push("/dashboard");
              })
              .catch(err => console.log(err));
          }}
        >
          clear
        </button>
      );
    }
  }

  render() {
    const { ticket } = this.props.location.state;

    console.log("TicketPage's ticket state ->", ticket);

    // TODO: verify user too
    if (typeof ticket !== "undefined" && ticket !== null) {
      return (
        <div>
          here is content -> {ticket.content} <br /> {ticket.ownerName}
          <br /> {this.renderDeleteButton(ticket)}
          <br /> {this.renderUpdateLink(ticket)}
        </div>
      );
    }

    return <div>Nothing here</div>;
  }
}

export default TicketPage;
