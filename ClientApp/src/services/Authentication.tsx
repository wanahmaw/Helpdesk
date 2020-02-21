import Axios from "axios";

// Get current user from session storage
const getCurrentUser = () => {
  const user = sessionStorage.getItem("user");
  if (user) {
    return JSON.parse(user);
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
        return null;
      })
  );
};

// Logout user
const logout = () => {
  sessionStorage.removeItem("user");
  alert("removed user!");
};

// Append authentication label to functions
export const authentication = {
  getCurrentUser,
  login,
  logout
};
