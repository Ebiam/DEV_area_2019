import * as React from 'react';
import {
  SafeAreaView,
  ScrollView,
  StatusBar,
  Text,
  View,
  TextInput,
  Button,
} from 'react-native';
import styles from '../styles/styles';

import '../global';

export default class ExempleView extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      username: '',
      password: '',
      content: null,
      loading: true,
      fieldError: false,
    };
  }

  componentDidMount(): void {
    if (this.state.loading === true) {
      fetch('https://jsonplaceholder.typicode.com/todos/1')
        .then(response => response.json())
        .then(json => {
          console.log(json);
          this.setState({content: json, loading: false});
        })
        .catch(error => console.log('error fetch :' + error));
    }
  }

  _onSingIn = () => {
    this.props.navigation.navigate('Dashboard');
  };

  _onPressButton = () => {
    console.log(
      'Username [' +
        this.state.username +
        '] Password [' +
        this.state.password +
        ']',
    );

    if (this.state.password == '' || this.state.username == '') {
      this.setState({fieldError: true});
      return;
    }
    var myHeaders = new Headers();
    myHeaders.append('Content-Type', 'application/json');
    myHeaders.append('Accept', 'application/json');
    var raw = JSON.stringify({"username":this.state.username, "password":this.state.password});
    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        dataType: 'json',
        redirect: 'follow',
    };
    fetch(global.serverUrl + "/user", requestOptions)
        .then(response => {
            if (response.status == 200)
                return (response.json());
        })
        .then(responseJson => {
                global.accountId = responseJson.id;
                global.username = responseJson.username;
            this.props.navigation.navigate('Dashboard');

        })
        .catch(error => console.log('error', error));
  };

  render() {
    var error =
      this.state.fieldError === true ? (
        <Text style={{color: 'red'}}>Both fields are required</Text>
      ) : null;
    return (
      <>
        <StatusBar barStyle="dark-content" />
        <SafeAreaView style={{flex:1}}>
          <ScrollView
            contentInsetAdjustmentBehavior="automatic"
            style={styles.scrollView}>
            <View style={styles.body}>
              <View style={styles.sectionContainer}>
                <TextInput
                  style={{height: 40}}
                  placeholder="Username"
                  onChangeText={username => this.setState({username})}
                  value={this.state.username}
                />
                <TextInput
                  style={{height: 40}}
                  placeholder="Password"
                  onChangeText={password => this.setState({password})}
                  value={this.state.password}
                />
                {error}
                <Button
                  onPress={this._onPressButton}
                  title="Log In"
                  color={'green'}
                />
                <Button onPress={this._onSingIn} title="No account ? Sign in" />
              </View>
            </View>
          </ScrollView>
        </SafeAreaView>
      </>
    );
  }
}
