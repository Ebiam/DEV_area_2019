import React from 'react';
import { withCookies, Cookies } from 'react-cookie';
import { instanceOf } from 'prop-types';


class Token2 extends React.Component {
    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };
    constructor(props) {
        super(props);
        const {cookies} = props;
        this.url = window.location.href;
        this.state = {
            link2: cookies.get('link2') || 'null'
        }
        this.urlParser(this.state.link2);
    }

    onSubmit = () => {
        this.props.history.push('/');
    }

    urlParser(id) {
        console.log("Id_Service: " + id);


        let url = this.url;
        let code = url.substring(url.indexOf("code=") + 5, url.length);
        if (id === "2") {
            var myHeaders = new Headers();
            myHeaders.append("Content-Type", "application/x-www-form-urlencoded");

            var urlencoded = new URLSearchParams();
            urlencoded.append("client_id", "efc261a3bef74cd395f334fe0639a723");
            urlencoded.append("client_secret", "f5988c3dab204d9a85990af17ee95b7b");
            urlencoded.append("grant_type", "authorization_code");
            urlencoded.append("code", code);
            urlencoded.append("redirect_uri", "http://localhost:8081/token2");

            var requestOptions = {
                method: 'POST',
                headers: myHeaders,
                body: urlencoded,
                redirect: 'follow'
            };
            fetch("https://accounts.spotify.com/api/token?redirect_uri=http://localhost:8081/token2&=https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify&redirect_uri=http://localhost:8081/token2",
                requestOptions).then(response => {
                return response.json();
            }).then(responceJson => {
                var token = responceJson.access_token;
                console.log(responceJson);
                var myHeaders = new Headers();
                myHeaders.append("Content-Type", "application/json");
                var raw = JSON.stringify({
                    userId: this.props.cookies.get('accountId'),
                    serviceId: id,
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
                        console.log("onsubmiiiiittt");
                        this.onSubmit();
                    })
                    .catch(error => console.log('error', error));
            }).catch(error => console.log('error', error));
        } else if (id === "4") {
            var formdata = new FormData();
            formdata.append("code", "4926b321783e3cb40d28");

            var requestOptions = {
                method: 'POST',
                body: formdata,
                redirect: 'follow'
            };
            var url2 = "https://github.com/login/oauth/access_token?client_id=c961210e5b7ec1a6ef85&client_secret=567f6b6891645292f907cc70a1d21628c0611659&code=" + code + "&redirect_uri=http://localhost:8081/token2";
            fetch(url2, requestOptions)
                .then(response => response.text())
                .then(result => {
                    const equal_index = result.indexOf("access_token=") + 13;
                    const and_index = result.indexOf("&scope=");
                    console.log(result);
                    console.log(result.substring(equal_index, and_index));
                    
                    var token = result.substring(equal_index, and_index);
                    var myHeaders = new Headers();
                    myHeaders.append("Content-Type", "application/json");
                    var raw = JSON.stringify({
                        userId: this.props.cookies.get('accountId'),
                        serviceId: id,
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
    }

    render() {
        return(
            <h1>En attente de redirection vers la page principale...</h1>
        );
    }
}
export default withCookies(Token2);
