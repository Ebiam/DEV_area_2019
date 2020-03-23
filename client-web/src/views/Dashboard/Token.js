import React from 'react';
import { withCookies, Cookies } from 'react-cookie';
import { instanceOf } from 'prop-types';


class Token extends React.Component {
    constructor(props) {
        super(props);
        this.url = window.location.href;
        const {cookies} = props;
        this.state = {
            ...this.state,
            response: "null",
            accountId: cookies.get('accountId') || '0',
            userName: cookies.get('userName') || '0',
            link2: cookies.get('link2') || 'null'
        }
        
        console.log(cookies.get('link2'));
        if (cookies.get('link2') === "1") {
            var code = this.url;
            const a = code.indexOf('=');
            code = code.substr(a + 1, 28);
            // code = code.substr();
            this.registerYammer(code);
            console.log(cookies.get('link2'));
        } else {
            this.urlParser();
        }
        
    }

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    registerYammer(code) {
        console.log("Yammer Code: " + code);
        var requestOptions = {
            method: 'POST',
            redirect: 'follow'
        };
        var url_yammer = "https://www.yammer.com/oauth2/access_token?client_id=HKeNI8GLJ6hZULHDY1twmQ&client_secret=0cRwx9Vd7nPoT9JfMsXHM2K2lVd3PRaHxsq6DPPNl8&code=" + code + "&grant_type=authorization_code";
        fetch(url_yammer, requestOptions)
            .then(response => response.json())
            .then(result => {
                console.log(result.access_token.token);
                var token = result.access_token.token;
                var myHeaders = new Headers();
                myHeaders.append("Content-Type", "application/json");
                var raw = JSON.stringify({
                    userId: this.props.cookies.get('accountId'),
		    serviceId: "1",
                    accessToken: token,
                    refreshToken: "",
                    username: "",
                    accountId: "0"
                });

                var requestOptions = {
		    method: 'POST',
		    headers: myHeaders,
		    body: raw,
		    redirect: 'follow'
                };

                fetch("http://localhost:8080/user/apiregister", requestOptions)
		    .then(response => {
                        if (response.status == 200)
			    return ("ok");
		    })
                    .then(responseJson => {
                        this.state.response = responseJson;
                        this.onSubmit();
		    })
                    .catch(error => console.log('error', error));
            }).catch(error => console.log('error', error));
    }

    urlParser() {
        let infoService = [];
        let info;
        let url = this.url;
        url = url.substring(url.indexOf("#") + 1, url.length);
        infoService = url.split("&");
        let listInfosImgur = [];
        let listInfosTrello = [];
        console.log(infoService);
        for (var cnt = 0; infoService[cnt]; cnt++)
        {
            
            info = infoService[cnt].substring(0, infoService[cnt].indexOf("="));
            
            if (info === 'access_token')
                    listInfosImgur.push(infoService[cnt].substring(infoService[cnt].indexOf("=") + 1, infoService[cnt].length));
            if (info === 'expires_in')
                    listInfosImgur.push(infoService[cnt].substring(infoService[cnt].indexOf("=") + 1, infoService[cnt].length));
            if (info === 'token_type')
                    listInfosImgur.push(infoService[cnt].substring(infoService[cnt].indexOf("=") + 1, infoService[cnt].length));
            if (info === 'refresh_token')
                    listInfosImgur.push(infoService[cnt].substring(infoService[cnt].indexOf("=") + 1, infoService[cnt].length));
            if (info === 'account_username')
                    listInfosImgur.push(infoService[cnt].substring(infoService[cnt].indexOf("=") + 1, infoService[cnt].length));
            if (info === 'account_id')
                    listInfosImgur.push(infoService[cnt].substring(infoService[cnt].indexOf("=") + 1, infoService[cnt].length));
            if (info === 'token')
                listInfosTrello.push(infoService[cnt].substring(infoService[cnt].indexOf("=") + 1, infoService[cnt].length));
            }
        if (listInfosImgur.length !== 0) {
            this.SendRequestImgur(listInfosImgur[0], listInfosImgur[3], listInfosImgur[4], listInfosImgur[5]);
        }
        else if (listInfosTrello.length !== 0)
        {
            this.SendRequestTrello(listInfosTrello[0]);
        }
    }

    onSubmit = () => {
       this.props.history.push('/');
    }

        SendRequestImgur(accessToken, refreshToken, accountUsername, accountId) {
            var myHeaders = new Headers();
            myHeaders.append("Content-Type", "application/json");
            var raw = JSON.stringify({
                userId: this.props.cookies.get('accountId'),
                serviceId: "5",
                accessToken: accessToken,
                refreshToken: refreshToken,
                username: accountUsername,
                accountId: accountId
            });
            var requestOptions = {
                method: 'POST',
                headers: myHeaders,
                body: raw,
                redirect: 'follow'
            };

            fetch("http://localhost:8080/user/apiregister", requestOptions)
                .then(response => {
                    if (response.status == 200)
                        return ("ok");
                })
                .then(responseJson => {
                    this.state.response = responseJson;
                    this.onSubmit();
                })
                .catch(error => console.log('error', error));
        }

    SendRequestTrello(token) {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        console.log(token);
        var raw = JSON.stringify({
                userId: this.props.cookies.get('accountId'),
                serviceId: "3",
                accessToken: token,
                refreshToken: "",
                username: this.props.cookies.get('aamama'),
                accountId: "0"
        });

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        fetch("http://localhost:8080/user/apiregister", requestOptions)
            .then(response => {
                if (response.status === 200)
                    return ("ok");
            })
            .then(responseJson => {
                this.state.response = responseJson;
                this.onSubmit();
            })
            .catch(error => console.log('error', error));
    }

    render() {
        return (
          <h1>En attente de redirection vers la page principale...</h1>
        );
    }
}

export default withCookies(Token);
