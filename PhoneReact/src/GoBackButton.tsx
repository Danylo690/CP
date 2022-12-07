import React from 'react';
import {Pressable, Text, View} from 'react-native';
import {useNavigate} from 'react-router-native';

const GoBackButton = () => {
  const navigate = useNavigate();
  return (
    <View
      style={{
        position: 'absolute',
        bottom: 0,
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'center',
        width: '100%',
      }}>
      <Pressable
        onPress={() => {
          navigate(-1);
        }}
        style={{
          
          borderRadius: 10,
          backgroundColor: 'gray',
          padding: 10,
          width: '50%',
        }}>
        <Text
          style={{
            color: 'blue',
            textAlign:'center',
          }}>
          ← Go Back
        </Text>
      </Pressable>
    </View>
  );
};

export default GoBackButton;
