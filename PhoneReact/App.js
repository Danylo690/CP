import React, { Component } from 'react'
import {
  StyleSheet,
  SafeAreaView,
  Button,
  TextInput,
  Text,
} from 'react-native'
import TcpSocket from 'react-native-tcp-socket';

class App extends Component {
  state =
  {
    text: 'not connected',
  }
  ConnectToServer = () => 
  {
    const client = TcpSocket.createConnection({
      port: 13000,
      host: '192.168.1.20',
      reuseAddress: true,
    });
    client.on('error', function(error) {
      console.log(error);
    });
  }

 render() {
    return (
      <SafeAreaView>
        <TextInput
        placeholder='Server IP'
        returnKeyLabel = {"next"}
        onChangeText={(text) => this.setState({host:text})}
        />
        <TextInput placeholder='Port'
        returnKeyLabel = {"next"}
        onChangeText={(text) => this.setState({port:parseInt(text)})}
        /> 
        <Button
        onPress={this.ConnectToServer}
        title="Connect"
        color="#841584"
        accessibilityLabel="Learn more about this purple button"
        />
        <Button
        //onPress={DisconnectFromServer}
        title="Disconnect"
        color="#841584"
        accessibilityLabel="Learn more about this purple button"
        />
        <Button
        //onPress={SendFile}
        title="Send files"
        color="#841584"
        accessibilityLabel="Learn more about this purple button"
        />
        <Text>{this.state.text}</Text>
      </SafeAreaView>
    )
  }
}

export default App;