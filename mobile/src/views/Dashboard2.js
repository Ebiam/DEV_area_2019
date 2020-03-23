import React from 'react';
import {
  Button,
  SafeAreaView,
  ScrollView,
  StatusBar,
  Text,
  TextInput,
  View,
} from 'react-native';
import {WebView} from 'react-native-webview';
import styles from '../styles/styles';
import DashboardStyles from '../styles/Dashboard';
import MySelector from '../components/MySelector';
import ServiceHeader from './ServiceHeader';
import '../global';
import logoSpotify from '../assets/img/logo-spotify.png';
import logoYammer from '../assets/img/logo-yammer.png';
import logoTrello from '../assets/img/logo-trello.png';
import logoGithub from '../assets/img/logo-github.png';
import logoImgur from '../assets/img/logo-imgur.png';
import Service from '../components/Service';
import '../global';

export default class DashboardView2 extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      name: '',
      password: '',
      tabHeader: '',
      getService: true,
      step: 0,
      serviceId: 0,
      url:
        'https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify&redirect_uri=http://localhost:8081/token2',
      webview: null,
      url2: '',
      actionId: '',
      actionParam: '',
      reactionId: '',
      reactionParam: '',
      ServiceList: [
        {
          id: 1,
          name: 'Yammer',
          logo: logoYammer,
          subscribed: false,
          link:
            'https://www.yammer.com/oauth2/authorize?client_id=HKeNI8GLJ6hZULHDY1twmQ&response_type=code&redirect_uri=http://localhost:8081/token&state=' +
            global.accountId +
            '_' +
            1,
          link2: '',
        },
        {
          id: 2,
          name: 'Spotify',
          logo: logoSpotify,
          subscribed: false,
          link:
            'https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify&redirect_uri=http://localhost:8081/token2',
          link2:
            'https://accounts.spotify.com/api/token?grant_type=authorization_code&redirect_uri=http://localhost:8081/token2&code=',
        },
        {
          id: 3,
          name: 'Trello',
          logo: logoTrello,
          subscribed: false,
          link:
            'https://trello.com/1/authorize?expiration=never&name=Area&scope=read,write,account&response_type=token&key=7b20ee1cf6225cb2dab5bcd4025c7d83&return_url=http://localhost:8081/token&state=' +
            global.accountId +
            '_' +
            3,
          link2: '',
        },
        {
          id: 4,
          name: 'Github',
          logo: logoGithub,
          subscribed: false,
          link:
            'https://github.com/login/oauth/authorize?client_id=c961210e5b7ec1a6ef85&redirect_uri=&http://localhost:8081/token2&scope=repo%20repo_deployment%20user%20public_repo' +
            global.accountId +
            '_' +
            4,
          link2:
            'https://github.com/login/oauth/access_token?client_id=c961210e5b7ec1a6ef85&client_secret=567f6b6891645292f907cc70a1d21628c0611659&code=4926b321783e3cb40d28&redirect_uri=http://localhost:8081/token2&state=' +
            global.accountId +
            '_' +
            4,
        },
        {
          id: 5,
          name: 'Imgur',
          logo: logoImgur,
          subscribed: false,
          link:
            'https://api.imgur.com/oauth2/authorize?response_type=token&client_id=41ebd49599744a4&client_secret=144c3ef1384ad0a5c239dd126f4ac06570bb8946&callback_url=http://localhost:8081/token&auth_url=https://api.imgur.com/oauth2/authorize&access_token_url=https://api.imgur.com/oauth2/token&state=' +
            global.accountId +
            '_' +
            5,
          link2: '',
        },
      ],
      sub: [],
      areas: [{actionId: 1, reactionId: 2}, {actionId: 2, reactionId: 1}],
      actions: [
        {id: 1, name: 'LOL', serviceId: 3, description: 'bite'},
        {id: 2, name: 'MDR', serviceId: 4, description: 'chien'},
      ],
      reactions: [
        {id: 1, name: 'LOL', serviceId: 3, description: 'poule'},
        {id: 2, name: 'MDR', serviceId: 4, description: 'salope'},
      ],
      userServices: [],
    };
  }

  get_from_url = (url, get) => {
    if (url === undefined || get === undefined) {
      return url;
    }
    let res = url.split(get);
    if (res.length < 2) {
      return url;
    }
    /*console.log('try Get ' + get + ' and got ' + res[1].split('&')[0]);*/
    return res[1].split('&')[0];
  };

  componentDidMount() {
    var web = (
      <WebView
        source={{
          uri: 'http://www.google.com',
        }}
        style={{marginTop: 20}}
        onNavigationStateChange={this.onNavChange}
      />
    );
    this.setState({webview: web});

    var services = [];
    var actions = [];
    var reactions = [];

    var myHeaders = new Headers();
    myHeaders.append('Content-Type', 'application/json');

    var raw = JSON.stringify({
      name: global.username,
    });

    var requestOptions = {
      method: 'POST',
      headers: myHeaders,
      body: raw,
      redirect: 'follow',
    };

    fetch(global.serverUrl + '/action')
      .then(response => {
        return response.json();
      })
      .then(awn => {
        actions = awn;
        fetch(global.serverUrl + '/reaction')
          .then(response => {
            return response.json();
          })
          .then(awn => {
            reactions = awn;
            fetch(global.serverUrl + '/service/getuser', requestOptions)
              .then(response => {
                return response.json();
              })
              .then(awn => {
                services = awn;
                actions = actions;/*.filter(a => services.includes(a.id));*/
                reactions = reactions;/*.filter(a => services.includes(a.id));*/
                this.setState({userServices: services, actions, reactions});
              })
              .catch(error => {
                console.log('error a' + error);
              });
          })
          .catch(error => {
            console.log('error b' + error);
          });
      })
      .catch(error => {
        console.log('error c' + error);
      });
  }

  SubscribeToService = elem => {
    var temp = this.state.ServiceList;
    temp[elem.id - 1].subscribed = true;
    //global.link2 = elem.id;
    //global.link = elem.link;
    //this.props.navigation.navigate('WebView');
    this.setState({
      step: 1,
      serviceId: elem.id,
      url: elem.link,
      url2: elem.link2,
      ServiceList: temp,
    });
    console.log(
      'Changed ' +
        temp[elem.id - 1].subscribed +
        'TO ' +
        this.state.ServiceList[elem.id - 1].subscribed,
    );
    this.setWebview(elem.link);
  };

  onNavChange = e => {
    console.log('Webview : URL CHANGED : ' + e.url);
    if (e.url.indexOf('http://localhost:8081/token2') === 0) {
      console.log('TOKEN 2 =================================================');
      let code = this.get_from_url(e.url, 'code=');
      console.log('CODE : ' + code);

      /* TODO fetch*/
      // TODO: FETCH et re setstate actions reactions
      if (this.state.serviceId.toString() === '2') {
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/x-www-form-urlencoded');

        var urlencoded = new FormData();
        var bobo = {
          'client_id': 'efc261a3bef74cd395f334fe0639a723',
          'client_secret': 'f5988c3dab204d9a85990af17ee95b7b',
          'code': code,
          'redirect_uri': 'http://localhost:8081/token2',
          'grant_type': 'authorization_code',
        };
        urlencoded.append('client_id', 'efc261a3bef74cd395f334fe0639a723');
        urlencoded.append('client_secret', 'f5988c3dab204d9a85990af17ee95b7b');

        urlencoded.append('code', code);
        urlencoded.append('redirect_uri', 'http://localhost:8081/token2');
        urlencoded.append('grant_type', 'authorization_code');

        let formBody = [];
        for (let property in bobo) {
          let encodedKey = encodeURIComponent(property);
          let encodedValue = encodeURIComponent(bobo[property]);
          formBody.push(encodedKey + "=" + encodedValue);
        }
        formBody = formBody.join("&");

        var requestOptions = {
          method: 'POST',
          headers: myHeaders,
          body: formBody,
          redirect: 'follow',
        };
        fetch(
          'https://accounts.spotify.com/api/token?redirect_uri=http://localhost:8081/token2&=https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify',
          requestOptions,
        )
          .then(response => {
            return response.json();
          })
          .then(res => {
            console.log('SPOT : ' + res.access_token + ' ' + res.error + ' ' + res.error_description);
            var token = res.access_token;

            var myHeaders = new Headers();
            myHeaders.append('Content-Type', 'application/json');
            var raw = JSON.stringify({
              userId: global.accountId.toString(),
              serviceId: this.state.serviceId.toString(),
              accessToken: token,
              refreshToken: '',
              username: '',
              accountId: '0',
            });

            var requestOptions = {
              method: 'POST',
              headers: myHeaders,
              body: raw,
              redirect: 'follow',
            };

            fetch(global.serverUrl + '/user/apiregister', requestOptions)
              .then(response => {
                if (response.status == 200) {
                  return 'ok';
                }
              })
              .then(responseJson => {
                /*this.state.response = responseJson;*/
                console.log('onsubmiiiiittt');
                this.setState({step: 0});
              })
              .catch(error => console.log('error localhost', error));
          })
          .catch(error => console.log('error spot', error));
      } else if (this.state.serviceId.toString() === '4') {
        var formdata = new FormData();
        formdata.append('code', '4926b321783e3cb40d28');

        var requestOptions = {
          method: 'POST',
          body: formdata,
          redirect: 'follow',
        };
        var url2 =
          'https://github.com/login/oauth/access_token?client_id=c961210e5b7ec1a6ef85&client_secret=567f6b6891645292f907cc70a1d21628c0611659&code=' +
          code +
          '&redirect_uri=http://localhost:8081/token2';
        fetch(url2, requestOptions)
          .then(response => response.text())
          .then(result => {
            const equal_index = result.indexOf('access_token=') + 13;
            const and_index = result.indexOf('&scope=');
            console.log(result);
            console.log(result.substring(equal_index, and_index));

            var token = result.substring(equal_index, and_index);
            console.log('YAMMMMMMMMS TOKEN ========' + token);
            var myHeaders = new Headers();
            myHeaders.append('Content-Type', 'application/json');
            var raw = JSON.stringify({
              userId: global.accountId.toString(),
              serviceId: this.state.serviceId.toString(),
              accessToken: token,
              refreshToken: '',
              username: '',
              accountId: '0',
            });

            var requestOptions = {
              method: 'POST',
              headers: myHeaders,
              body: raw,
              redirect: 'follow',
            };

            fetch(global.serverUrl + '/user/apiregister', requestOptions)
              .then(response => {
                if (response.status == 200) {
                  return 'ok';
                }
              })
              .then(responseJson => {
                /*this.state.response = responseJson;*/
                this.setState({step: 0});
              })
              .catch(error => console.log('error localhost', error));
          })
          .catch(error => console.log('error', error));
      }
      this.setState({step: 0});


    } else if (e.url.indexOf('http://localhost:8081/token') === 0) {
      console.log('TOKEN 1 =================================================');
      let accessToken = this.get_from_url(e.url, 'token=');

      console.log(
        'ACCESS TOKEN FOR SERVICE ' +
          this.state.serviceId.toString() +
          ' : ' +
          accessToken,
      );

      /* TODO fetch Tu a l'id du service sur this.state.serviceId*/
      // TODO: FETCH et re setstate actions reactions

      if (this.state.serviceId.toString() === '1') {
        let code = this.get_from_url(e.url, 'code=');
        console.log('YAMMMMMMMEEEEERRRR code ' + code);
        var requestOptions = {
          method: 'POST',
          redirect: 'follow',
        };
        var url_yammer =
          'https://www.yammer.com/oauth2/access_token?client_id=HKeNI8GLJ6hZULHDY1twmQ&client_secret=0cRwx9Vd7nPoT9JfMsXHM2K2lVd3PRaHxsq6DPPNl8&code=' +
          code +
          '&grant_type=authorization_code';
        fetch(url_yammer, requestOptions)
          .then(response => response.json())
          .then(result => {
            console.log('YAMS' + result.access_token.token);
            var token = result.access_token.token;
            var myHeaders = new Headers();
            myHeaders.append('Content-Type', 'application/json');
            var raw = JSON.stringify({
              userId: global.accountId.toString(),
              serviceId: this.state.serviceId.toString(),
              accessToken: token,
              refreshToken: '',
              username: '',
              accountId: '0',
            });

            var requestOptions = {
              method: 'POST',
              headers: myHeaders,
              body: raw,
              redirect: 'follow',
            };

            fetch(global.serverUrl + '/user/apiregister', requestOptions)
              .then(response => {
                if (response.status == 200) {
                  return 'ok';
                }
              })
              .then(responseJson => {
                /*this.state.response = responseJson;*/
                this.setState({step: 0});
              })
              .catch(error => console.log('error localhost', error));
          })
          .catch(error => console.log('error', error));
      } else {
        var myHeaders = new Headers();
        myHeaders.append('Content-Type', 'application/json');
        var raw = JSON.stringify({
          userId: global.accountId.toString(),
          serviceId: this.state.serviceId.toString(),
          accessToken: accessToken,
          refreshToken: '',
          username: '',
          accountId: '0',
        });

        var requestOptions = {
          method: 'POST',
          headers: myHeaders,
          body: raw,
          redirect: 'follow',
        };
        fetch(global.serverUrl + '/user/apiregister', requestOptions)
          .then(response => {
            if (response.status === 200) {
              return 'ok';
            }
          })
          .then(responseJson => {
            /*this.state.response = responseJson;*/
            console.log(
              'ACCESS TOKEN FOR SERVICE ' +
                this.state.serviceId.toString() +
                ' DOOOOONE',
            );
            this.setState({step: 0});
          })
          .catch(error => console.log('error', error));
      }
    } else if (e.url.indexOf('https://api.imgur.com/oauth2/authorize') == -1) {
      //this.setState({key: this.state.key + 1});
    }
  };

  setWebview(link) {
    var web = (
      <WebView
        source={{
          uri: link,
        }}
        style={{marginTop: 20}}
        onNavigationStateChange={this.onNavChange}
      />
    );
    this.setState({webview: web});
  }

  onPressButton() {
    console.log('PRESSED');
    console.log('LOL ');
    if (true) {
      // TODO => fetch this.state.actionId this.state.actionParam this.state.reactionId this.state.reactionParam global.accountId
      var myHeaders = new Headers();
      myHeaders.append('Content-Type', 'application/json');
      console.log('LOL ');
      var raw = JSON.stringify({
        userId: global.accountId.toString(),
        actionId: this.state.actionId,
        actionParam: this.state.actionParam,
        reactionId: this.state.reactionId,
        reactionParam: this.state.reactionParam,
      });

      var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow',
      };
      fetch(global.serverUrl + '/area', requestOptions)
        .then(response => {
          if (response.status === 200) {
            return 'ok';
          }
        })
        .then(responseJson => {
          /*this.state.response = responseJson;*/
          console.log('AREA REPONSE' + responseJson.toString());
          this.setState({actionId: '', actionParam: '', reactionId: '', reactionParam: ''});
        })
        .catch(error => console.log('error', error));
    }
  }

  render() {
    if (this.state.step == 1) {
      return <View style={{flex: 1}}>{this.state.webview}</View>;
    }

    return (
      <>
        <StatusBar barStyle="dark-content" />
        <SafeAreaView style={{flex: 1}}>
          <ScrollView
            contentInsetAdjustmentBehavior="automatic"
            style={styles.scrollView}>
            <View style={styles.body}>
              <View style={styles.sectionContainer}>
                <Text>{this.state.actionId}</Text>

                {this.state.ServiceList.map(elem => (
                  <View style={DashboardStyles.containerfluid}>
                    <Text style={DashboardStyles.textServiceHeader}>
                      {elem.name}
                    </Text>
                    <Button
                      onPress={() => {
                        this.SubscribeToService(elem);
                      }}
                      title={
                        elem.subscribed == false ? 'Activer' : 'Désactiver'
                      }
                    />
                  </View>
                ))}
                {/*
                  <ServiceHeader
                    id={this.state.ServiceList[0].id}
                    name={this.state.ServiceList[0].name}
                    subscribed={this.state.ServiceList[0].subscribed}
                    link={this.state.ServiceList[0].link}
                    link2={this.state.ServiceList[0].link2}
                  />
                  <ServiceHeader
                    id={this.state.ServiceList[1].id}
                    name={this.state.ServiceList[1].name}
                    subscribed={this.state.ServiceList[0].subscribed}
                    link={this.state.ServiceList[1].link}
                    link2={this.state.ServiceList[1].link2}
                  />
                  <ServiceHeader
                    id={this.state.ServiceList[2].id}
                    name={this.state.ServiceList[2].name}
                    subscribed={this.state.ServiceList[0].subscribed}
                    link={this.state.ServiceList[2].link}
                    link2={this.state.ServiceList[2].link2}
                  />
                  <ServiceHeader
                    id={this.state.ServiceList[3].id}
                    name={this.state.ServiceList[3].name}
                    subscribed={this.state.ServiceList[0].subscribed}
                    link={this.state.ServiceList[3].link}
                    link2={this.state.ServiceList[3].link2}
                  />
                  <ServiceHeader
                    id={this.state.ServiceList[4].id}
                    name={this.state.ServiceList[4].name}
                    subscribed={this.state.ServiceList[0].subscribed}
                    link={this.state.ServiceList[4].link}
                    link2={this.state.ServiceList[4].link2}
                  />
                  */}
                <Text>Actions</Text>
                {this.state.actions.map(elem => (
                  <View>
                    <Text> id : {elem.id} </Text>
                    <Text> nom : {elem.name} </Text>
                    <Text> description :{elem.description} </Text>
                  </View>
                ))}
                <Text>Reactions</Text>
                {this.state.reactions.map(elem => (
                  <View>
                    <Text> id : {elem.id} </Text>
                    <Text> nom : {elem.name} </Text>
                    <Text> description : {elem.description} </Text>
                  </View>
                ))}
                <Text>Créer une Area</Text>
                <View>
                  <TextInput
                    style={{height: 40}}
                    placeholder="Id de l'action"
                    onChangeText={actionId =>
                      this.setState({actionId: actionId})
                    }
                    value={this.state.actionId}
                  />
                  <TextInput
                    style={{height: 40}}
                    placeholder="Param de l'action (si demandé)"
                    onChangeText={actionParam =>
                      this.setState({actionParam: actionParam})
                    }
                    value={this.state.actionParam}
                  />
                  <TextInput
                    style={{height: 40}}
                    placeholder="Id de la reaction"
                    onChangeText={reactionId =>
                      this.setState({reactionId: reactionId})
                    }
                    value={this.state.reactionId}
                  />
                  <TextInput
                    style={{height: 40}}
                    placeholder="Param de la réaction (si demandé)"
                    onChangeText={reactionParam =>
                      this.setState({reactionParam: reactionParam})
                    }
                    value={this.state.reactionParam}
                  />
                  <Button
                    onPress={this.onPressButton.bind(this)}
                    title="Créer"
                    color={'green'}
                  />
                </View>
                <Text>Mes Areas</Text>
                {this.state.areas.map(elem => (
                  <View>
                    <Text> Action : {} </Text>
                    <Text> </Text>
                  </View>
                ))}
              </View>
            </View>
          </ScrollView>
        </SafeAreaView>
      </>
    );
  }
}
