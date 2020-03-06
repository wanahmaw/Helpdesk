import Axios from "axios";

// Get current user from session storage
const getCurrentUser = () => {
  const user = sessionStorage.getItem("user");
  if (user) {
    return JSON.parse(user);
  }
  return null;
};

// Get current user's role
const getUserRole = () => {
  const user = getCurrentUser();
  if (user && user.token && user.roleId) {
    return user.roleId;
  }
  return null;
};

// Get current user ID
const getUserId = () => {
  const user = getCurrentUser();
  if (user && user.token && user.userId) {
    return user.userId;
  }
  return null;
};

// Login user
const login = (username: string, password: string) => {
  return (
    Axios.post("/api/login", {
      username,
      password
    })
      // Save user in session
      .then(res => {
        if (res && res.status === 201) {
          sessionStorage.setItem("user", JSON.stringify(res.data));
          console.log("WE LOGGED IN");
          return res.data;
        }
      })
      .catch(err => {
        console.log(err);
        alert("Invalid, please try again");
      })
  );
};

// Logout user
const logout = () => {
  sessionStorage.removeItem("user");
  alert("removed user!");
};

// Get authorization header for authorized routes in back-end
const getAuthHeader = () => {
  const user = getCurrentUser();
  if (user && user.token) {
    return { Authorization: `Bearer ${user.token}` };
  } else {
    return {};
  }
};

// Append authentication label to functions
export const authentication = {
  getCurrentUser,
  login,
  logout,
  getAuthHeader,
  getUserRole,
  getUserId
};
