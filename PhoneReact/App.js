import React, { Component } from 'react'
import {
  StyleSheet,
  SafeAreaView,
  Button,
  TextInput,
  Text,
  NativeModules,
  PermissionsAndroid
} from 'react-native'
import TcpSocket from 'react-native-tcp-socket';
import FilePicker from 'react-native-document-picker';
import {base64 } from "rfc4648";
import RNFS from "react-native-fs";

const {FTPServer} = NativeModules;

class App extends Component {
  constructor(props)
  {
    super(props);
    this.state =
    {
      client: new TcpSocket.Socket,
      fileSent: false,
      host: '192.168.1.20',
      port: 13000,
      isConnected: false,
      text: '',
    }
    this.fileSentOptions =
    {
      name: [],
      path: [],
      NumberOfPacket: [],
      currentNumberOfPacket: new Number,
      currentFileInBytes: new Uint8Array,
      currentLength: new Number,
    }

    this.myRef = React.createRef();
    this.ConnectToServer = this.ConnectToServer.bind(this);
    this.DisconnectFromServer = this.DisconnectFromServer.bind(this);
    this.SwitchButton = this.SwitchButton.bind(this);
    this.PickFiles = this.PickFiles.bind(this);
    this.SendFile = this.SendFile.bind(this);
    this.CreateFTPServer = this.CreateFTPServer.bind(this);
  }

  CreateFTPServer()
  {
    FTPServer.StartFTPServer();
    this.setState({text: "FTP server is running"});
  }

  async ConnectToServer() 
  {
    this.state.client = TcpSocket.createConnection({
      port: this.state.port,
      host: this.state.host,
      reuseAddress: true,
      noDelay: true,
      keepAlive: true,
    });
    const Func1 = (data) => 
    {
      console.log('message was received', data.toString('utf8'));
      this.SendFile(data.toString('utf8'));
    }
    this.state.client.on('connect', () => {this.SwitchButton(); console.log(this.state.isConnected)});
    this.state.client.on('data', (data) => Func1(data));
    
    this.state.client.on('error', function(error) {
      console.log(error);
    });
    
    this.state.client.on('close', function(){
      console.log('Connection closed!');
    });
    this.state.client.on('pause', () => {console.log("pause")})
    this.state.client.on('drain', () => console.log("blablabla"))
  }

  SwitchButton()
  {
    this.state.isConnected = !this.state.isConnected;
    this.setState({isConnected: this.state.isConnected});
  }

  DisconnectFromServer()
  {
    this.state.client.write("<Disconnect>");
    this.state.client.on('data', () => this.state.client.destroy());
    this.state.client.on('close', () => this.SwitchButton());
  }

  async PickFiles()
  {
    try
    {
      const bufferSize = 1024;
      let pickedFiles = await FilePicker.pickMultiple({ presentationStyle: 'fullScreen'});
      pickedFiles.forEach(element => {
        this.fileSentOptions.name.push(element.name);
        this.fileSentOptions.path.push(element.uri);
        this.fileSentOptions.NumberOfPacket.push(Math.ceil(parseFloat(element.size) / parseFloat(bufferSize)));
      });
      this.state.client.write("Start sending files\r\n");
    }
    catch (err) 
    {
      if (FilePicker.isCancel(err)) {
        console.log("Canceled from picker")
      }
    }
  }

  async SendFile(command)
  {
    let name = this.fileSentOptions.name[this.fileSentOptions.name.length-1];
    const bufferSize = 1024;
    let start = 0;
    let end = 0;

    switch(command)
    {
      case "NeedMsg": 
        {
          this.state.client.write("Sending file " + name + "\r\n");
          break;
        }
      case "NeedName": 
        { 
          console.log(name);
          this.fileSentOptions.currentFileInBytes = base64.parse(await RNFS.readFile(this.fileSentOptions.path.pop(), "base64"));
          this.fileSentOptions.name.pop();
          this.fileSentOptions.currentLength = this.fileSentOptions.currentFileInBytes.length;
          this.fileSentOptions.currentNumberOfPacket = 0;
          console.log(name);
          this.state.client.write(name);
          break;
        }
      case "NeedNumberOfPackets":
        {
          console.log(this.fileSentOptions.NumberOfPacket);
          this.state.client.write(this.fileSentOptions.NumberOfPacket.pop().toString());
          break;
        }
      case "NeedPacket":
        {
          if (this.fileSentOptions.currentLength > bufferSize)
          {
            start = this.fileSentOptions.currentNumberOfPacket * bufferSize;
            end = (this.fileSentOptions.currentNumberOfPacket + 1) * bufferSize;
            this.fileSentOptions.currentLength = this.fileSentOptions.currentLength - bufferSize;
          }
          else
          {
            start = (this.fileSentOptions.currentNumberOfPacket) * bufferSize;
            end = parseInt(this.fileSentOptions.currentFileInBytes.length);
          }
          console.log(start + ": " + end);
          this.state.client.write(this.fileSentOptions.currentFileInBytes.slice(start, end));
          this.fileSentOptions.currentNumberOfPacket++;
          break;
        }
      case "WaitNextFile":
        {
          if(this.fileSentOptions.name.length)
          {
            this.state.client.write("Send next file");
          }
          break;
        }
    }
  }

  render() {
    return (
      <SafeAreaView>
        <TextInput
        placeholder='Server IP'
        onChangeText={(text) => this.setState({host: text})}
        />
        <TextInput
        placeholder='Port'
        onChangeText={(text) => this.setState({port: parseInt(text)})}
        /> 
        <Button
        ref={this.myRef}
        disabled={this.state.isConnected == true?true:false}
        onPress={this.ConnectToServer.bind(this)}
        title="Connect"
        color="#841584"
        accessibilityLabel="Learn more about this purple button"
        />
        <Button
        disabled={!this.state.isConnected}
        onPress={this.DisconnectFromServer.bind(this)}
        title="Disconnect"
        color="#841584"
        accessibilityLabel="Learn more about this purple button"
        />
        <Button
        //disabled={!this.state.isConnected}
        onPress={this.PickFiles}
        title="Send files"
        color="#841584"
        accessibilityLabel="Learn more about this purple button"
        />
        <Button
        onPress={this.CreateFTPServer}
        title="Create FTP server"
        color="#841584"
        accessibilityLabel="Learn more about this purple button"
        />
        <Text>{this.state.text}</Text>
      </SafeAreaView>
    )
  }
}

export default App;