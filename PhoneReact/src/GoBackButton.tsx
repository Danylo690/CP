import React from 'react';
import {Pressable, Text, View, Keyboard} from 'react-native';
import {useNavigate} from 'react-router-native';

const GoBackButton = () => {
  const navigate = useNavigate();
  const [keyboardVisible, setKeyboardVisible] = React.useState(false);

  React.useEffect(() => {
    const keyboardDidShowListener = Keyboard.addListener(
      'keyboardDidShow',
      () => {
        setKeyboardVisible(true);
      },
    );
    const keyboardDidHideListener = Keyboard.addListener(
      'keyboardDidHide',
      () => {
        setKeyboardVisible(false);
      },
    );

    return () => {
      keyboardDidHideListener.remove();
      keyboardDidShowListener.remove();
    };
  }, []);

  if (keyboardVisible) return <></>;

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
          backgroundColor: '#007fff',
          padding: 10,
          width: '50%',
        }}>
        <Text
          style={{
            color: 'white',
            textAlign: 'center',
            fontSize: 18,
          }}>
          {'< Go Back'}
        </Text>
      </Pressable>
    </View>
  );
};

export default GoBackButton;
