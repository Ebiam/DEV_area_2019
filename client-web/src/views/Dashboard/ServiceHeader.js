import React from "react";

import { withCookies, Cookies } from 'react-cookie';
import { instanceOf } from 'prop-types';


import "../../assets/css/Dashboard/Dashboard.css";

class ServiceHeader extends React.Component {
    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    SubscribeToService = () => {
        const {cookies} = this.props;
        cookies.set('link2', this.props.id);
        window.location = this.props.link;
    }

    async UnsubscribeToService(props) {

    }

    IsSubscribed(id) {
        let jsxTab = [];
        if(this.props.subscribed === false)
            jsxTab.push(<button onClick={this.SubscribeToService} className="button-header-true">Activer</button>);
        else
            jsxTab.push(<button onSubmit={this.UnsubscribeToService} className="button-header-false">Desactiver</button>);
        return (jsxTab);
    }

    render() {
        return (
            <div className="card card-stats mb-4 mb-xl-0">
                <div className="card-body">
                    <div className="row">
                        <div className="col">
                            <h5 className="card-title text-uppercase text-muted mb-0">AREA</h5>
                            <span className="h2 font-weight-bold mb-0">{this.props.name}</span>
                        </div>
                        <div className="col-auto">
                            <d
                                className="icon icon-shape bg-danger text-white rounded-circle shadow">
                                <img className="logo-header" src={this.props.logo}/>
                                <i className="fas fa-chart-bar"></i>
                            </d>
                        </div>
                    </div>
                    <p className="mt-3 mb-0 text-muted text-sm">
                        {this.IsSubscribed()}
                    </p>
                </div>
            </div>
        );
    }
}
export default withCookies(ServiceHeader);