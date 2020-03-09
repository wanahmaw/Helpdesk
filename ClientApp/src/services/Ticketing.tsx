import Axios from "axios";
import { authentication } from "./Authentication";
import { History } from "history";

const onSuccess = "/dashboard";

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

const addTicket = (values: any, history: History) => {
  // Get user detail
  const ownerId = authentication.getUserId();
  const url = "/api/ticket";
  // Post to backend
  Axios.post(
    url,
    {
      title: values.title,
      content: values.content,
      ownerId
    },
    {
      headers: authentication.getAuthHeader()
    }
  )
    .then(data => {
      // Back to homepage
      if (data && data.status === 201) {
        history.push(onSuccess);
      }
    })
    .catch(err => console.log(err));
};

const updateTicket = (values: any, ticketId: number, history: History) => {
  // Get user detail
  const ownerId = authentication.getUserId();
  const url = `/api/ticket/${ticketId}`;
  // Update to backend
  Axios.put(
    url,
    {
      title: values.title,
      content: values.content,
      ownerId
    },
    {
      headers: authentication.getAuthHeader()
    }
  )
    .then(data => {
      // Back to homepage
      if (data && data.status === 204) {
        history.push(onSuccess);
      }
    })
    .catch(err => console.log(err));
};

const deleteTicket = (ticketId: number, history: History) => {
  // Get role
  const roleId = authentication.getUserRole();
  const url = `/api/ticket/${ticketId}`;
  // Delete if you're a team member
  if (roleId === 2) {
    Axios.delete(url, {
      headers: authentication.getAuthHeader()
    })
      .then(data => {
        // Back to homepage
        if (data && data.status === 200) {
          console.log("deleteTicket successful, returning data");
          history.push(onSuccess);
        }
      })
      .catch(err => console.log(err));
  }
};

export const ticketing = {
  getTicketsByRole,
  deleteTicket,
  addTicket,
  updateTicket
};
