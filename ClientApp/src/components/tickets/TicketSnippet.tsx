import React, { Component } from "react";
import { Link } from "react-router-dom";

interface IProps {
  ticket: any;
}

class TicketSnippet extends Component<IProps> {
  render() {
    const { ticket } = this.props;

    // Display title, owner, date, MAYBE a little piece of the data here
    return (
      <li className="collection-item">
        <div>
          <Link
            to={{
              pathname: `/ticket/${ticket.ticketId}`,
              state: {
                ticket
              }
            }}
          >
            <span className="blue-text text-darken-3">{ticket.title}</span>
          </Link>
          <br /> {ticket.ownerName}
        </div>
      </li>
    );
  }
}

export default TicketSnippet;
