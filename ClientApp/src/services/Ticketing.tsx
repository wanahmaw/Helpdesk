import Axios from "axios";
import { authentication } from "./Authentication";

const getSnippet = async (ticketId: number) => {
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

export const ticketing = {
  getSnippet,
  getTicketsByRole
};
