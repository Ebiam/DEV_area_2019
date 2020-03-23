import Dashboard from "@material-ui/icons/Dashboard";
import Login from "@material-ui/icons/Person";
import Signin from '@material-ui/icons/Create';
import Error from '@material-ui/icons/Error';

import DashboardPage from "client-web/src/views/Dashboard/Dashboard.js";
import LoginPage from "client-web/src/views/Login/Login.js";
import SigninPage from "client-web/src/views/Sign in/Sign in.js";
import ErrorPage from "client-web/src/views/Error/Error404";

const AreaRoutes = [
    {
        path: "/dashboard",
        name: "Dashboard",
        icon: Dashboard,
        component: DashboardPage,
    },
    {
        path: "/login",
        name: "Login",
        icon: Login,
        component: LoginPage,
    },
    {
        path: "/signin",
        name: "Sign in",
        icon: Signin,
        component: SigninPage,
    },
    {
        path: "*",
        name: "Error 404",
        icon: Error,
        component: ErrorPage
    }
];