import React from 'react';
import {
  Text,
  NativeModules,
  Pressable,
  View,
  TextInput,
  Image,
} from 'react-native';
import {NetworkInfo} from 'react-native-network-info';
import Clipboard from '@react-native-clipboard/clipboard';
// import FontAwesome,{SolidIcons,RegularIcons,BrandIcons}from'react-native-fontawesome';
// import { FontAwesomeIcon } from '@fortawesome/react-native-fontawesome'
// import { faMugSaucer } from '@fortawesome/free-solid-svg-icons/faMugSaucer'
const {FTPServer} = NativeModules;

const ServerPage = () => {
  const [FTPStartText, setFTPText] = React.useState('');
  const [FTPIPText, setFTPIPText] = React.useState('');
  const [CopyText, setCopyText] = React.useState(
    'You can control your device from PC after start this service',
  );
  const [TextOnOf, setTextOnOf] = React.useState('Create FTP server');
  const [FTPIsCreated, setFTPIsCreated] = React.useState(true);

  //setTextOnOf('Create FTP server');
  //setFTPText('');
  //setCopyText('You can control your device from PC after start this service');

  // let flag = true;
  function SetMyIpAddress() {
    NetworkInfo.getIPV4Address().then(ipAddress => {
      setFTPIPText(`ftp://${ipAddress}:2121/`); //ftp://192.168.1.15:2121/
    });
  }
  function createFTPServer() {
    setFTPIsCreated(false);
    FTPServer.StartFTPServer();
    setFTPText('FTP server is running');
    SetMyIpAddress();
    setCopyText('Enter this address in your PC');
    setTextOnOf('Close FTP server');

    //console.log(flag);
  }
  function CloseFTPServer() {
    setFTPIsCreated(true);

    FTPServer.EndFTPServer();

    setTextOnOf('Create FTP server');
    setFTPText('');
    setFTPIPText('');
    setCopyText('You can control your device from PC after start this service');

    //console.log(flag);
  }

  function handleLinkCopy() {
    Clipboard.setString(FTPIPText);
    setCopyText('Copied to clipboard ✔️');
  }

  return (
    <View
      style={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        width: '100%',
        height: '100%',
      }}>
      <Text>Server Page</Text>

      <Image
        style={{width: 75, aspectRatio: 1, marginTop: '50%', marginBottom: 25}}
        source={{
          uri: 'https://cdn.discordapp.com/attachments/163342524839231497/1051491527127465984/wifi_image.png',
        }}
      />

      <Pressable
        onPress={FTPIsCreated === true ? createFTPServer : CloseFTPServer}
        style={{
          backgroundColor: '#007fff',
          padding: 10,
          borderRadius: 10,
          width: '75%',
          display: 'flex',
          shadowColor: '#000',
          shadowOffset: {width: 0, height: 2},
          shadowOpacity: 1,
          shadowRadius: 5,
          elevation: 10,
        }}>
        <Text
          style={{
            textAlign: 'center',
            color: 'white',
            fontSize: 20,
          }}>
          {TextOnOf}
        </Text>
      </Pressable>

      <View
        style={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          width: '100%',
          flexDirection: 'column',
        }}>
        <Text
          style={{
            textAlign: 'center',
            color: '#555',
            fontSize: 18,
          }}>
          {FTPStartText}
        </Text>
        <Text
          style={{
            textAlign: 'center',
            color: '#555',
            fontSize: 18,
          }}>
          {CopyText}
        </Text>

        {FTPIPText && (
          <Pressable
            style={{
              borderColor: '#007fff',
              borderWidth: 1,
              padding: 10,
              width: '75%',
              display: 'flex',
              flexDirection: 'row',
              justifyContent: 'space-between',
              alignItems: 'center',
            }}
            onPress={handleLinkCopy}>
            <Text
              style={{
                textAlign: 'center',
                color: '#000',
                fontSize: 20,
              }}>
              {FTPIPText}
            </Text>
            <Image
              style={{width: 20, aspectRatio: 1}}
              source={{
                uri: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fcdn.onlinewebfonts.com%2Fsvg%2Fimg_365643.png&f=1&nofb=1&ipt=3a45151d97d13d76051a5818209d34b0b7dc231bf4c544d76903eeead059afcc&ipo=images',
              }}
            />
          </Pressable>
        )}
      </View>
    </View>
  );
};

export default ServerPage;
