import React from 'react';
import {View, Text, Pressable} from 'react-native';

type Props = {
  onCreate: () => void;
};
const ClientHeader = ({onCreate}: Props) => {
  return (
    <View
      style={{
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        width: '100%',
        zIndex: 10,
        height: 50,
        backgroundColor: 'white',
      }}>
      <Text
        style={{
          fontSize: 20,
          fontWeight: 'bold',
          color: '#333',
        }}>
        Client Page
      </Text>
      <Pressable
        style={{
          backgroundColor: 'white',
          padding: 5,
          borderRadius: 10,
          width: '30%',
          display: 'flex',
          borderColor: 'grey',
          borderWidth: 2,
        }}
        onPress={onCreate}>
        <Text
          style={{
            textAlign: 'center',
            color: 'black',
          }}>
          Create
        </Text>
      </Pressable>
    </View>
  );
};

export default ClientHeader;
