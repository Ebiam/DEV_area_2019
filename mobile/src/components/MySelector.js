import React from 'react';
import {View, Text, Button} from 'react-native';

export default class MySelector extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isPressed: false,
    };
  }

  _onPress = () => {
    console.log('de');
  };

  render() {
    return (
      <View style={{flex: 1, flexDirection: 'row'}}>
        <View style={{flex: 1}}>
          <Button onPress={this._onPress} title="Press Me" />
        </View>
        <View style={{flex: 5, backgroundColor: '#02343430'}}>
          {this.props.children.map(elem => (
            <Text>{elem.a}</Text>
          ))}
        </View>
      </View>
    );
  }
}
