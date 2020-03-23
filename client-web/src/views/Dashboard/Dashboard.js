//REACT LIB
import React from "react";

//CSS
import "../../assets/css/Dashboard/Dashboard.css";
import "../../assets/css/styles.css";
import "../../assets/css/Dashboard/ServiceBody.css";
import "../../assets/css/Dashboard/area.css";

//LAYOUT
import topBar from  "../../layouts/topBar";

import ServiceHeader from "./ServiceHeader";
import Area from "./Area";

//VIEWS
import ActionReaction from "./ActionReaction";

//ASSETTS
import logoSpotify from "../../assets/img/logo-spotify.png";
import logoYammer from "../../assets/img/logo-yammer.png";
import logoTrello from "../../assets/img/logo-trello.png";
import logoGithub from "../../assets/img/logo-github.png";
import logoImgur from "../../assets/img/logo-imgur.png";

import { withCookies, Cookies } from 'react-cookie';
import { instanceOf } from 'prop-types';


class Dashboard extends React.Component {
    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };
    constructor(props) {
        super(props);
        const {cookies} = props;
        this.state = {
            accountId: cookies.get('accountId') || '0',
            userName: cookies.get('userName') || '0',
            link2: cookies.get('link2') || '0',
            initArea: true,
            loaded: true,
            getService: true,
            tasks: [],
            tabHeader: null,
            tabOption: [],
            ServiceList: [
                {
                    id: "1",
                    name: "Yammer",
                    logo: logoYammer,
                    subscribed: false,
                    link: "https://www.yammer.com/oauth2/authorize?client_id=HKeNI8GLJ6hZULHDY1twmQ&response_type=code&redirect_uri=http://localhost:8081/token&state=" + this.props.accountId + "_" + 1,
                    link2:""
                },
                {
                    id: "2",
                    name: "Spotify",
                    logo: logoSpotify,
                    subscribed: false,
                    link: "https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify&redirect_uri=http://localhost:8081/token2",
                    link2: "https://accounts.spotify.com/api/token?grant_type=authorization_code&redirect_uri=http://localhost:8081/token2&code="
                },
                {
                    id: "3",
                    name: "Trello",
                    logo: logoTrello,
                    subscribed: false,
                    link: "https://trello.com/1/authorize?expiration=never&name=Area&scope=read,write,account&response_type=token&key=7b20ee1cf6225cb2dab5bcd4025c7d83&return_url=http://localhost:8081/token&state=" + this.props.cookies.get('accountId') + "_" + 3,
                    link2:""
                },
                {
                    id: "4",
                    name: "Github",
                    logo: logoGithub,
                    subscribed: false,
                    link: "https://github.com/login/oauth/authorize?client_id=c961210e5b7ec1a6ef85&redirect_uri=&http://localhost:8081/token2&scope=repo%20repo_deployment%20user%20public_repo" + this.props.cookies.get('accountId') + "_" + 4,
                    link2: "https://github.com/login/oauth/access_token?client_id=c961210e5b7ec1a6ef85&client_secret=567f6b6891645292f907cc70a1d21628c0611659&code=4926b321783e3cb40d28&redirect_uri=http://localhost:8081/token2&state=" + this.props.accountId + "_" + 4
                },
                {
                    id: "5",
                    name: "Imgur",
                    logo: logoImgur,
                    subscribed: false,
                    link: "https://api.imgur.com/oauth2/authorize?response_type=token&client_id=41ebd49599744a4&client_secret=144c3ef1384ad0a5c239dd126f4ac06570bb8946&callback_url=http://localhost:8081/token&auth_url=https://api.imgur.com/oauth2/authorize&access_token_url=https://api.imgur.com/oauth2/token&state=" + this.props.cookies.get('accountId') + "_" + 5,
                    link2:""
                }
            ],
            sub: [],
            area: [],
        };
        this.checkifLog();
    }

    checkifLog() {
        if (this.state.accountId == '0')
            this.props.history.push('/signin');
    }

    isSubscribed() {
        for (var cnt = 0;this.state.ServiceList[cnt];cnt++) {
            this.state.ServiceList[cnt].subscribed = false;
            for (var cnt2 = 0; this.state.sub[cnt2]; cnt2++) {
                if (this.state.sub[cnt2] == cnt + 1) {
                    this.state.ServiceList[cnt].subscribed = true;
                }
            }
        }
    }

    getSubscribed()
    {
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        var raw = JSON.stringify({"name": this.state.userName});
        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            dataType: 'json',
            redirect: 'follow',
        };
        fetch("http://localhost:8080/service/getuser", requestOptions)
            .then(response => {
                if (response.status == 200)
                    return (response.json());
            })
            .then(responseJson => {
                this.state.sub = responseJson;
                this.isSubscribed();
                this.setState({
                    ...this.state,
                    tabHeader: this.DisplayServices(),
                    getService: false,
                });
            })
            .catch(error => console.log('error', error));
    }

    DisplayServices() {
        let jsxTab = [];
        for (var service = 0; this.state.ServiceList[service]; service++) {
            jsxTab.push(<div className="service-header-3 service-header-6"><ServiceHeader id={this.state.ServiceList[service].id} name={this.state.ServiceList[service].name} logo={this.state.ServiceList[service].logo} subscribed={this.state.ServiceList[service].subscribed} link={this.state.ServiceList[service].link} link2={this.state.ServiceList[service].link2}/></div>);
        }
        return (jsxTab);
    }

    GetOptionsService(ActionOrReaction, idService, service, ActionReactionCnt, idDnd) {
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        var requestOptions = {
            method: 'GET',
            headers: myHeaders,
            dataType: 'json',
            redirect: 'follow',
        };
        fetch("http://localhost:8080/" + ActionOrReaction + "/" + idService, requestOptions)
            .then(response => {
                if (response.status === 200) {
                    return response.json();
                }
            })
            .then(responseJson => {
                var tmp = this.state.tasks;
                tmp.push(new ActionReaction(service, ActionReactionCnt, idDnd, responseJson, ActionOrReaction));
                this.setState({
                    ...this.state,
                    loaded: false,
                    tasks: tmp
                });
            })
            .catch(error => console.log('error', error));
    }

    initDragAndDropVar()
    {
        let tasks = [];
        let idDnd = 0;
        let ActionOrReaction;
        for (var service = 0; this.state.ServiceList[service]; service++) {
            if (this.state.ServiceList[service].subscribed == true) {
                for (var ActionReactionCnt = 0; ActionReactionCnt != 2; ActionReactionCnt++) {
                    if (ActionReactionCnt == 0)
                        ActionOrReaction = "Action";
                    else
                        ActionOrReaction = "Reaction";
                    this.GetOptionsService(ActionOrReaction, this.state.ServiceList[service].id, this.state.ServiceList[service], ActionReactionCnt, idDnd);
                    idDnd++;
                }
            }
        }
        return (tasks);
    }

    onDragStart = (ev, id) => {
        ev.dataTransfer.setData("id", id);
    }

    onDragOver = (ev) => {
        ev.preventDefault();
    }

    onDrop = (ev, cat) => {
        let id = ev.dataTransfer.getData("id");
        let tasks = this.state.tasks.filter((task) => {
            if (task.getId() == id) {
                if ((task.getDefaultCategory() == "Action" && cat == "DropAction") || (task.getDefaultCategory() == "Reaction" && cat == "DropReaction"))
                    task.setCategory(cat);
            }
            return (task);
        });

        this.setState({
            ...this.state,
            tasks
        });
    }

    hideDrop(compo, who) {
        let jsxTab = [];
        if (compo.length == 0)
            jsxTab.push(<h1>Drop {who}</h1>);
        return (jsxTab);
    }

    createRequestArea(Action, Reaction) {
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        var raw = JSON.stringify({
            userId: this.state.accountId,
            actionId: Action.getCurrentOption(),
            actionParam: Action.getCurrentOptionInput(),
            reactionId: Reaction.getCurrentOption(),
            reactionParam:Reaction.getCurrentOptionInput()
        });
        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            dataType: 'json',
            redirect: 'follow',
        };
        fetch("http://localhost:8080/area", requestOptions)
            .then(response => {
                    window.location = "http://localhost:8081/";
            })
            .then(responseJson => {
            })
            .catch(error => console.log('error', error));
    }

    createArea() {
        let Action;
        let Reaction;
        for (var cnt = 0; this.state.tasks[cnt]; cnt++) {
            if (this.state.tasks[cnt].getCategory() == "DropAction")
                Action = this.state.tasks[cnt];
            if (this.state.tasks[cnt].getCategory() == "DropReaction")
                Reaction = this.state.tasks[cnt];
        }
        if (Action == null && Reaction == null)
            return (<h1>Drop Action and Reaction for create area</h1>)
        else if (Action == null)
            return (<h1>Drop Action for create area</h1>)
        else if (Reaction == null)
            return (<h1>Drop Reaction for create area</h1>)
        if (Action != null && Reaction != null) {
            this.createRequestArea(Action, Reaction);
        }
    }

    getRequestArea()
    {
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        var requestOptions = {
            method: 'GET',
            headers: myHeaders,
            dataType: 'json',
            redirect: 'follow',
        };
        fetch("http://localhost:8080/area", requestOptions)
            .then(response => {
                if (response.status === 200) {
                    return response.json();
                }
            })
            .then(responseJson => {
                let tmp = [];
                let infosAction = [];
                let infosReaction = [];
                for (var cnt = 0; responseJson[cnt]; cnt++) {
                    infosAction = this.getInfosActions(responseJson[cnt].actionId);
                    infosReaction = this.getInfosReactions(responseJson[cnt].reactionId);

                    if (infosAction !== null || infosReaction !== null)
                        tmp.push(<Area areaId={responseJson[cnt].id} infosActionParams={responseJson[cnt].actionParam} infosActionsName={infosAction.name} infosActionsLogo={infosAction.logo} infosReactionParams={responseJson[cnt].reactionParam} infosReactionsName={infosReaction.name} infosReactionsLogo={infosReaction.logo}></Area>);
                }
                if (infosAction.length !== 0 || infosReaction.length !== 0)
                    this.setState({...this.state, area: tmp, initArea:false});
            })
            .catch(error => console.log('error', error));
    }

    getInfosActions(actionId)
    {
        let infosAction = {
            name: "anyName",
            category: "anyName",
            logo: "anyName",
        };
        let tmp = [];
        for (var cnt = 0; this.state.tasks[cnt]; cnt++)
        {
            tmp = this.state.tasks[cnt].getOptionList();
            for (var cnt2 = 0; tmp[cnt2]; cnt2++)
            {
                if (tmp[cnt2].id === actionId && this.state.tasks[cnt].getDefaultCategory() === "Action")
                {
                    infosAction.name = this.state.tasks[cnt].getName();
                    infosAction.category = this.state.tasks[cnt].getDefaultCategory();
                    infosAction.logo = this.state.tasks[cnt].getLogo();
                    return (infosAction);
                }
            }
        }
        return (null);
    }

    getInfosReactions(reactionId)
    {
        let infosReaction = {
            name: "anyName",
            category: "anyName",
            logo: "anyName",
        };
        let tmp = [];
        for (var cnt = 0; this.state.tasks[cnt]; cnt++)
        {
            tmp = this.state.tasks[cnt].getOptionList();
            for (var cnt2 = 0; tmp[cnt2]; cnt2++)
            {
                if (tmp[cnt2].id === reactionId && this.state.tasks[cnt].getDefaultCategory() === "Reaction")
                {
                    infosReaction.name = this.state.tasks[cnt].getName();
                    infosReaction.category = this.state.tasks[cnt].getDefaultCategory();
                    infosReaction.logo = this.state.tasks[cnt].getLogo();
                    return (infosReaction);
                }
            }
        }
        return (null);
    }

    render() {
        var task = {
            Action: [],
            Reaction:[],
            DropAction: [],
            DropReaction: []
        }

        this.state.tasks.forEach ((t) => {
            task[t.getCategory()].push(<div key={t.getId()}
                                            onDragStart = {(e) => this.onDragStart(e, t.getId())}
                                            draggable>{t.dispBlock()}</div>);
        });

        if (this.state.getService === true)
        {
            this.getSubscribed();
        }

        if (this.state.loaded === true)
        {
            this.initDragAndDropVar();
        }

        if (this.state.initArea === true)
        {
            this.getRequestArea();
        }

        return(
            <div>
                <div className="header bg-gradient-primary pb-8 pt-5 pt-md-8">
                    <div className="topbar">{topBar}</div>
                    <div className="container-fluid">
                        <div className="header-body">
                            <div className="row">
                                {this.state.tabHeader}
                            </div>
                        </div>
                    </div>
                    <a href="http://localhost:8081/getapp">Download App</a>
                </div>
                <div className="service-body">
                    <h1 className="service-title">Mes Services</h1>
                    <row className="service-row">
                        <div className="service-action">
                            <h2>Mes Actions</h2>
                            <div className="Action"
                                 onDragOver={(e)=>this.onDragOver(e)}
                                 onDrop={(e)=>{this.onDrop(e, "Action")}}>
                                {task.Action}
                            </div>
                        </div>
                        <div className="service-reaction">
                            <h2>Mes Reactions</h2>
                            <div className="Reaction"
                                 onDragOver={(e)=>this.onDragOver(e)}
                                 onDrop={(e)=>{this.onDrop(e, "Reaction")}}>
                                {task.Reaction}
                            </div>
                        </div>
                    </row>
                    <h1 className="service-title">Mon Area</h1>
                    <div className="area-drop-case">
                        <div className="action-drop-case" onDragOver={(e)=>this.onDragOver(e)}
                             onDrop={(e)=>{this.onDrop(e, "DropAction")}}>
                            {this.hideDrop(task.DropAction, "Action")}
                            {task.DropAction}
                        </div>
                        <button className="button-create-area" onClick={this.createArea.bind(this)}>Create Area</button>
                        <div className="reaction-drop-case" onDragOver={(e)=>this.onDragOver(e)}
                             onDrop={(e)=>{this.onDrop(e, "DropReaction")}}>
                            {this.hideDrop(task.DropReaction, "Reaction")}
                            {task.DropReaction}
                        </div>
                    </div>
                    <div className="area-corp">
                        <h1 className="mes-areas">Mes Areas</h1>
                        {this.state.area}
                    </div>
                    <div className="agrandissement"></div>
                </div>
            </div>
        );
    }
}

export default withCookies(Dashboard);
