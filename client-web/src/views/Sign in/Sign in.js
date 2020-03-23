import React from "react";
import "../../assets/css/register.css";
import AreaLogo from "../../assets/img/area-logo.png";

import { withCookies, useCookies, Cookies } from 'react-cookie';
import { instanceOf } from 'prop-types';

class Signin extends React.Component {
    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);
        this.SendRequest = this.SendRequest.bind(this);
        this.state = {
            response: "null",
            loggedIn: false
        }
        const { cookies } = props;
        this.state = {
            accountId: cookies.get('accountId') || '',
            userName: cookies.get('userName') || '',
            link2: cookies.get('link2') || '',
        };
    }
    onSubmit = () => {
        this.props.history.push('/');
    }

    setCookies(accountId, userName) {
        const { cookies } = this.props;
        cookies.set('accountId', accountId);
        cookies.set('userName', userName);
        cookies.set('link2', "http");
    }

    SendRequest(event) {
        event.preventDefault();
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        myHeaders.append('Accept', 'application/json');
        var raw = JSON.stringify({"username":event.target.elements.username.value, "password":event.target.elements.pass.value});
        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            dataType: 'json',
            redirect: 'follow',
        };
        fetch("http://localhost:8080/user", requestOptions)
            .then(response => {
                if (response.status == 200)
                    return (response.json());
            })
            .then(responseJson => {
                    this.state.response = responseJson;
                    this.setCookies(responseJson.id, responseJson.username);
                    this.onSubmit();
            })
            .catch(error => console.log('error', error));
    }

    render() {
        return (
            <div className="background-form">
                <div className="wrap-login100">
                    <form className="login100-form validate-form" onSubmit={this.SendRequest}>
                    <span className="login100-form-logo">
                        <i className="zmdi zmdi-landscape"/>
                        <img className="logo-header-form" src={AreaLogo}/>
                    </span>
                        <span className="login100-form-title p-b-34 p-t-27">
                        Sign in
                    </span>
                        <div className="wrap-input100 validate-input" data-validate="Enter username">
                            <input className="input100" type="text" name="username" placeholder="Username"/>
                            <span className="focus-input100"/>
                        </div>
                        <div className="wrap-input100 validate-input" data-validate="Enter password">
                            <input className="input100" type="password" name="pass" placeholder="Password"/>
                            <span className="focus-input100"/>
                        </div>
                        <div className="wrap-input100 validate-input" data-validate="Confirm password">
                            <input className="input100" type="password" name="confpass" placeholder="Confirm password"/>
                            <span className="focus-input100"/>
                        </div>
                        <div className="container-login100-form-btn">
                            <button className="login100-form-btn">
                                Sign in
                            </button>
                        </div>
                    </form>
                    <a href="http://localhost:8081/login/">Already an account ?</a>
                </div>
            </div>
        );
    }
}

export default withCookies(Signin);
