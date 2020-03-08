import Axios from "axios";
import { authentication } from "./Authentication";

// TODO: Remove? Not really used
const getTicket = async (ticketId: number) => {
  const url = `/api/ticket/${ticketId}`;
  // Get snippet
  const snippet = await Axios.get(url, {
    headers: authentication.getAuthHeader()
  });
  // Return snippet
  if (snippet) {
    return snippet.data;
  }
};

const getTicketsByRole = async () => {
  // Get user details
  const roleId = authentication.getUserRole();
  const userId = authentication.getUserId();
  let url;
  // Use proper endpoint
  roleId === 1 ? (url = `api/ticket/user/${userId}`) : (url = `api/ticket`);
  // Get tickets
  const tickets = await Axios.get(url, {
    headers: authentication.getAuthHeader()
  });
  // Return tickets
  if (tickets) {
    return tickets.data;
  }
};

const deleteTicket = async (ticketId: number) => {
  // Get role
  const roleId = authentication.getUserRole();
  const url = `/api/ticket/${ticketId}`;
  if (roleId == 2) {
    Axios.delete(url, {
      headers: authentication.getAuthHeader()
    }).then(data => {
      if (data && data.status == 200) {
        return data;
      }
    });
  }
};

export const ticketing = {
  getTicket,
  getTicketsByRole,
  deleteTicket
};
