import React, {Component, useState, useRef} from 'react';
import {
  StyleSheet,
  SafeAreaView,
  Button,
  TextInput,
  Text,
  NativeModules,
  PermissionsAndroid,
  View,
} from 'react-native';
import TcpSocket from 'react-native-tcp-socket';
import FilePicker from 'react-native-document-picker';
import {base64} from 'rfc4648';
import RNFS from 'react-native-fs';
import {NavigationContainer} from '@react-navigation/native';
import HomePage from './HomePage';
import Router from './Router';

import {NativeRouter} from 'react-router-native';

const {FTPServer} = NativeModules;

const App = props => {
  const [state, setState] = useState({
    client: new TcpSocket.Socket(),
    fileSent: false,
    host: '192.168.1.20',
    port: 13000,
    isConnected: false,
    text: '',
    UseState: true,
  });

  const [fileSentOptions, setFileSentOptions] = useState({
    name: [],
    path: [],
    NumberOfPacket: [],
    currentNumberOfPacket: new Number(),
    currentFileInBytes: new Uint8Array(),
    currentLength: new Number(),
  });

  const [showActions, setShowActions] = useState('');

  const myRef = useRef(null);

  function toggleActionsShow(action) {
    setShowActions(action);
  }

  function createFTPServer() {
    FTPServer.createFTPServer();
    setState();
  }

  async function connectToServer() {
    const client = TcpSocket.createConnection({
      port: state.port,
      host: state.host,
      reuseAddress: true,
      noDelay: true,
      keepAlive: true,
    });

    client.on('connect', () => {
      switchButton();
      console.log(state.isConnected);
    });

    client.on('data', data => {
      console.log('message was received', data.toString('utf8'));
      sendFile(data.toString('utf8'));
    });

    client.on('error', error => {
      console.log('error', error);
    });

    client.on('close', () => {
      console.log('connection closed');
    });

    client.on('pause', () => {
      console.log('connection paused');
    });

    client.on('drain', () => {
      console.log('blablabla');
    });

    setState(prev => ({...prev, client: client}));
  }

  function switchButton() {
    setState(prev => ({...prev, isConnected: !prev.isConnected}));
  }

  function disconnectFromServer() {
    state.client.write('<Disconnect>');
    state.client.on('data', () => state.client.destroy());
    state.client.on('close', switchButton);
  }

  async function pickFiles() {
    try {
      const bufferSize = 1024;
      let pickedFiles = await FilePicker.pickMultiple({
        presentationStyle: 'fullScreen',
      });

      pickedFiles.forEach(element => {
        fileSentOptions.name.push(element.name);
        fileSentOptions.path.push(element.uri);
        fileSentOptions.NumberOfPacket.push(
          Math.ceil(parseFloat(element.size) / parseFloat(bufferSize)),
        );
      });

      state.client.write('Start sending files\r\n');
    } catch (err) {
      if (FilePicker.isCancel(err)) {
        console.log('Canceled from picker');
      }
    }
  }

  async function sendFile(command) {
    const name = fileSentOptions.name[fileSentOptions.name.length - 1];
    const bufferSize = 1024;
    let start = 0;
    let end = 0;
    switch (command) {
      case 'NeedMsg': {
        state.client.write('Sending file ' + name + '\r\n');
        break;
      }
      case 'NeedName': {
        console.log(name);

        const file = await RNFS.readFile(fileSentOptions.path.pop(), 'base64');
        const fileInBytes = base64.parse(file);

        fileSentOptions.currentFileInBytes = fileInBytes;
        fileSentOptions.name.pop();
        fileSentOptions.currentLength = fileInBytes.length;
        fileSentOptions.currentNumberOfPacket = 0;

        console.log(name);

        state.client.write(name);
        break;
      }
      case 'NeedNumberOfPackets': {
        const pocketCount = fileSentOptions.NumberOfPacket;

        console.log(pocketCount);

        state.client.write(pocketCount.pop().toString());
        break;
      }
      case 'NeedPacket': {
        const currentPacketCount = fileSentOptions.currentNumberOfPacket;

        if (fileSentOptions.currentLength > bufferSize) {
          start = currentPacketCount * bufferSize;
          end = (currentPacketCount + 1) * bufferSize;
          fileSentOptions.currentLength -= bufferSize;
        } else {
          start = currentPacketCount * bufferSize;
          end = parseInt(fileSentOptions.currentFileInBytes.length);
        }

        console.log(start + ': ' + end);

        const fileSlice = fileSentOptions.currentFileInBytes.slice(start, end);

        state.client.write(fileSlice);

        setFileSentOptions(prev => ({
          ...prev,
          currentNumberOfPacket: prev.currentNumberOfPacket + 1,
        }));
        break;
      }
      case 'WaitNextFile': {
        if (fileSentOptions.name.length) {
          state.client.write('Send next file');
        }
        break;
      }
    }
  }

  return (
    <NativeRouter>
      <Router />
    </NativeRouter>
  );

  // return (
  //   <SafeAreaView>
  //     <HomePage />
  //     <Button
  //       title="Being client"
  //       color="#841584"
  //       onPress={() => toggleActionsShow('client')}
  //     />

  //     <Button
  //       title="Start ftp server"
  //       color="#841584"
  //       onPress={() => toggleActionsShow('server')}
  //     />

  //     {showActions === 'client' && (
  //       <>
  //         <TextInput
  //           placeholder="Server IP"
  //           onChangeText={text => setState(prev => ({...prev, host: text}))}
  //         />
  //         <TextInput
  //           placeholder="Port"
  //           onChangeText={text => this.setState({port: parseInt(text)})}
  //         />
  //         <Button
  //           ref={this.myRef}
  //           disabled={this.state.isConnected == true ? true : false}
  //           onPress={this.ConnectToServer.bind(this)}
  //           title="Connect"
  //           color="#841584"
  //           accessibilityLabel="Learn more about this purple button"
  //         />
  //         <Button
  //           disabled={!this.state.isConnected}
  //           onPress={this.DisconnectFromServer.bind(this)}
  //           title="Disconnect"
  //           color="#841584"
  //           accessibilityLabel="Learn more about this purple button"
  //         />
  //         <Button
  //           //disabled={!this.state.isConnected}
  //           onPress={this.PickFiles}
  //           title="Send files"
  //           color="#841584"
  //           accessibilityLabel="Learn more about this purple button"
  //         />
  //       </>
  //     )}
  //     {showActions === 'Server' && (
  //       <>
  //         <Button
  //           onPress={this.CreateFTPServer}
  //           title="Create FTP server"
  //           color="#841584"
  //           accessibilityLabel="Learn more about this purple button"
  //         />

  //         <Text>{this.state.text}</Text>
  //       </>
  //     )}
  //   </SafeAreaView>
  // );
};

export default App;
