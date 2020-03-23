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

export default class DashboardView extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      name: '',
      password: '',
      tabHeader: '',
      getService: true,
      ServiceList: [
        {
          id: '1',
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
          id: '2',
          name: 'Spotify',
          logo: logoSpotify,
          subscribed: false,
          link:
            'https://accounts.spotify.com/authorize?response_type=code&client_id=efc261a3bef74cd395f334fe0639a723&scope=user-read-playback-state%20ugc-image-upload%20user-read-playback-state%20user-modify-playback-state%20user-read-currently-playing%20streaming%20app-remote-control%20user-read-email%20user-read-private%20playlist-read-collaborative%20playlist-modify-public%20playlist-read-private%20playlist-modify-private%20user-library-modify%20user-library-read%20user-top-read%20user-read-recently-played%20user-follow-read%20user-follow-modify&redirect_uri=http://localhost:8081/token2',
          link2:
            'https://accounts.spotify.com/api/token?grant_type=authorization_code&redirect_uri=http://localhost:8081/token2&code=',
        },
        {
          id: '3',
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
          id: '4',
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
          id: '5',
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
    };
  }

  componentDidMount(): void {}

  _onPressButton = () => {
    console.log('on press');
  };

  getSubscribed() {
    var myHeaders = new Headers();
    myHeaders.append('Content-Type', 'application/json');
    var raw = JSON.stringify({name: global.username});
    var requestOptions = {
      method: 'POST',
      headers: myHeaders,
      body: raw,
      dataType: 'json',
      redirect: 'follow',
    };
    fetch(global.serverUrl + '/service/getuser', requestOptions)
      .then(response => {
        if (response.status == 200) {
          return response.json();
        }
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

  isSubscribed() {
    for (var cnt = 0; this.state.ServiceList[cnt]; cnt++) {
      this.state.ServiceList[cnt].subscribed = false;
      for (var cnt2 = 0; this.state.sub[cnt2]; cnt2++) {
        if (this.state.sub[cnt2] === cnt + 1) {
          this.state.ServiceList[cnt].subscribed = true;
        }
      }
    }
  }

  DisplayServices() {
    let jsxTab = [];
    for (var service = 0; this.state.ServiceList[service]; service++) {
      jsxTab.push(
        <ServiceHeader
          id={this.state.ServiceList[service].id}
          name={this.state.ServiceList[service].name}
          subscribed={this.state.ServiceList[service].subscribed}
          link={this.state.ServiceList[service].link}
          link2={this.state.ServiceList[service].link2}
        />,
      );
    }
    return jsxTab;
  }

  render() {
    if (this.state.getService === true) {
      this.getSubscribed();
    }
    return (
      <>
        <StatusBar barStyle="dark-content" />
        <SafeAreaView>
          <ScrollView
            contentInsetAdjustmentBehavior="automatic"
            style={styles.scrollView}>
            <View style={styles.body}>
              <View style={styles.sectionContainer}>
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
              </View>
            </View>
          </ScrollView>
        </SafeAreaView>
      </>
    );
  }
}
