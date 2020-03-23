import React from "react";
import "../../assets/css/register.css";
import AreaLogo from "../../assets/img/area-logo.png"
import {connect} from 'react-redux';

class Login  extends React.Component {
    constructor() {
        super();
        this.state = {
            username: null
        }
    }
    _storeLogin(username, password, accountId, accessToken, serviceId, accountServiceId)
    {
        const action = {
            type: 'LOGIN',
            value : {
                username: username,
                password: password,
                accountId: accountId,
                accessToken: accessToken,
                serviceId: serviceId,
                accountServiceId: accountServiceId
            }
        };
        this.props.dispatch(action);
    }
    SendRequest(event) {
        event.preventDefault();
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        myHeaders.append('Accept', 'application/json');
        var raw = JSON.stringify({
            "username": event.target.elements.username.value,
            "password": event.target.elements.pass.value
        });
        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            dataType: 'json',
            redirect: 'follow',
        };
        fetch("http://localhost:8080/user", requestOptions)
            .then(response => {
                if (response.status === 200)
                    return (response.json());
            })
            .then(responseJson => {
                this.state.response = responseJson;
                this._storeLogin(responseJson.id, responseJson.name, responseJson.password);
                this.onSubmit();
            })
            .catch(error => console.log('error', error));
    }
    render() {

        return (
            <div className="background-form">
                <div className="wrap-login100">
                    <form className="login100-form validate-form">
                        <span className="login100-form-logo">
                            <i className="zmdi zmdi-landscape"/>
                            <img className="logo-header-form" src={AreaLogo}/>
                        </span>
                        <span className="login100-form-title p-b-34 p-t-27">Log in </span>
                        <div className="wrap-input100 validate-input" data-validate="Enter username">
                            <input className="input100" type="text" name="username" placeholder="Username"/>
                            <span className="focus-input100"/>
                        </div>
                        <div className="wrap-input100 validate-input" data-validate="Enter password">
                            <input className="input100" type="password" name="pass" placeholder="Password" value={this.state.username}/>
                            <span className="focus-input100"/>
                        </div>
                        <div className="container-login100-form-btn">
                            <button className="login100-form-btn">Log in</button>
                        </div>
                    </form>
                    <a href="http://localhost:8081/signin/">Any account ?</a>
                </div>
            </div>
        );
    }
}

const currentState = state => {
    return state;
}

export default connect(currentState)(Login);