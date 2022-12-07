import React from 'react';
import {Text, NativeModules, Pressable, View} from 'react-native';

const {FTPServer} = NativeModules;

const ServerPage = () => {
  const [text, setText] = React.useState('');

  function createFTPServer() {
    FTPServer.StartFTPServer();
    setText('FTP server is running');
  }
  return (
    <>
      <Text>Server Page</Text>
      <Text>{text}</Text>
      <View
        style={{
          position: 'absolute',
          bottom: 10,
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          width: '100%',
        }}>
        <Pressable
          onPress={() => createFTPServer()}
          style={{
            backgroundColor: 'blue',
            padding: 10,
            borderRadius: 10,
            width: '75%',
            display: 'flex',
          }}>
          <Text
            style={{
              textAlign: 'center',
            }}>
            Create FTP server
          </Text>
        </Pressable>
      </View>
    </>
  );
};

export default ServerPage;
