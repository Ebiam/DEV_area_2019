import React from "react";
import "../../assets/css/Dashboard/area.css";
import "../../assets/css/Dashboard/Actions.css";

import logoSpotify from "../../assets/img/logo-spotify.png";
import logoYammer from "../../assets/img/logo-yammer.png";
import logoTrello from "../../assets/img/logo-trello.png";
import logoGithub from "../../assets/img/logo-github.png";
import logoImgur from "../../assets/img/logo-imgur.png";
import { withCookies, useCookies, Cookies } from 'react-cookie';
import { instanceOf } from 'prop-types';

export class Area extends React.Component {
    constructor(props) {
        super(props);
    }
    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    deleteArea (areaId)
    {
        // console.log();
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({"areaId":areaId.toString()});

        var requestOptions = {
            method: 'DELETE',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        fetch("http://localhost:8080/area", requestOptions)
            .then(response => response.text())
            .then(result => window.location = "http://localhost:8081/")
            .catch(error => console.log('error', error));
        ;
        // this.props.history.push('/');
    }

    render() {
        return (
            <div className="area-body">
                <div className="actions">
                    <div className="action-block-areas">
                        <div>
                            <h5  className="text-uppercase action-text align-text-area">Action</h5>
                            <div className="align-text-area">{this.props.infosActionsName}</div>
                        </div>
                        <div className="div-logo-Action align-text-area">
                            <img className="logo-action" src={this.props.infosActionsLogo}/>
                        </div>
                            <div className="align-text-area">{this.props.infosActionParams}</div>
                        </div>
                    </div>
                <button onClick={this.deleteArea.bind(this, this.props.areaId)} className="supp-area">Supprimer</button>
                <div className="actions">
                    <div className="action-block-area">
                        <div>
                            <h5  className="text-uppercase action-text align-text-area">Reaction</h5>
                            <div className="align-text-area">{this.props.infosReactionsName}</div>
                        </div>
                        <div className="div-logo-Action align-text-area">
                            <img className="logo-action" src={this.props.infosReactionsLogo}/>
                        </div>
                        <div className="align-text-area">{this.props.infosReactionParams}</div>
                    </div>
                </div>
            </div>
        );
    }
}

export default withCookies(Area);
