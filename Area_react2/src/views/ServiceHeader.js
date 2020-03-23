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
import DashboardStyles from '../styles/Dashboard';

import '../global';

export default class ServiceHeader extends React.Component {
  constructor(props) {
    super(props);
  }

  SubscribeToService = () => {
    global.link2 = this.props.id;
    global.link = this.props.link;
    this.props.navigation.navigate('WebView');
  };

  IsSubscribed(id) {
    let jsxTab = [];
    if (this.props.subscribed === false) {
      jsxTab.push(<Button onPress={this.SubscribeToService} title="Activer" />);
    } else {
      jsxTab.push(<Button title="Desactiver" />);
    }
    return jsxTab;
  }

  render() {
    return (
      <View style={DashboardStyles.containerfluid}>
        <Text style={DashboardStyles.textServiceHeader}>{this.props.name}</Text>
        <View>{this.IsSubscribed()}</View>
      </View>
    );
  }
}
