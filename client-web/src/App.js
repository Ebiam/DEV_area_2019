import React from 'react';
import { BrowserRouter, Route } from "react-router-dom";
import DashboardRoute from "./views/Dashboard/Dashboard";
import LoginRoute from "./views/Login/Login";
import SigninRoute from "./views/Sign in/Sign in";
import TokenRoute from './views/Dashboard/Token';
import Token2Route from './views/Dashboard/Token2';
import AreaRoute from './views/Dashboard/Area';
import GetAppRoute from './views/Dashboard/GetApp'
import { CookiesProvider } from 'react-cookie';

class App extends React.Component {
    render() {
        return (
            <CookiesProvider>
                    <BrowserRouter>
                        <Route exact path="/" component={DashboardRoute}/>
                        <Route exact path="/login" component={LoginRoute}/>
                        <Route exact path="/token" component={TokenRoute}/>
                        <Route exact path="/token2" component={Token2Route}/>
                        <Route exact path="/signin" component={SigninRoute}/>
                        <Route exact path="/area" component={AreaRoute}/>
                        <Route exact path="/getapp" component={GetAppRoute}/>
                    </BrowserRouter>
            </CookiesProvider>
        );
    }
}

export default App;
