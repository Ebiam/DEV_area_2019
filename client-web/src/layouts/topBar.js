import React from "react";

function topBar() {
    return (
        <div className="navbar navbar-top navbar-expand-md navbar-dark" id="navbar-main">
            <div className="container-fluid">
                <a className="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block"
                   href="http://localhost:8081/">Dashboard</a>
                <ul className="navbar-nav align-items-center d-none d-md-flex">
                    <li className="nav-item dropdown">
                        <a className="nav-link pr-0" href="http://localhost:8081/" role="button" data-toggle="dropdown" aria-haspopup="true"
                           aria-expanded="false">
                            <div className="media align-items-center">
                                <div className="media-body ml-2 d-none d-lg-block">
                                </div>
                            </div>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    );
}

export default topBar();