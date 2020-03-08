import React, { Component } from "react";
import { History } from "history";
import TicketSnippet from "./tickets/TicketSnippet";
import { Link } from "react-router-dom";
import { ticketing } from "../services/Ticketing";

interface State {
  // TODO: match scheme and interface
  tickets: any;
}

interface Props {
  history: History;
}

export class Dashboard extends Component<Props, State> {
  constructor(props: any) {
    super(props);

    this.state = {
      tickets: []
    };
  }

  componentDidMount() {
    // Get tickets by role
    ticketing.getTicketsByRole().then(tickets => {
      this.setState({
        tickets
      });
    });
  }

  renderTicketSnippets() {
    // Display snippets when available
    if (this.state.tickets && this.state.tickets.length > 0) {
      return this.state.tickets.map((ticket: any) => (
        <ul key={ticket.ticketId}>
          <TicketSnippet ticket={ticket} key={ticket.ticketId} />
        </ul>
      ));
    }

    // Show this when there's nothing to display or loading takes a while
    return (
      <li className="collection-item">
        <h3>No tickets to display</h3>
      </li>
    );
  }

  static displayName = Dashboard.name;

  render() {
    return (
      <div>
        <h2>Help Desk</h2>
        <ul className="collection with-header">
          <li className="collection-header lighten-5 blue">
            <h3>Tickets</h3>
            <span></span>
            <Link
              className="center waves-effect waves-light orange btn"
              to={{
                pathname: "/ticket-new"
              }}
            >
              Add Ticket
            </Link>
          </li>
          {this.renderTicketSnippets()}
        </ul>
      </div>
    );
  }
}
