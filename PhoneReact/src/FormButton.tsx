import React from 'react';
import {Pressable, Text, StyleSheet} from 'react-native';

const FormButton = ({
  label,
  onPress,
  id,
}: {
  label: string;
  onPress: () => void;
  id: number;
}) => {
  const styles = StyleSheet.create({
    action: {
      width: '50%',
      // backgroundColor: 'red',
      padding: 10,
      alignItems: 'center',
      justifyContent: 'center',
      borderWidth: 1,
      borderColor: '#007fff',
      borderBottomWidth: 0,
      borderLeftWidth: id ? 1 : 0,
      borderRightWidth: id ? 0 : 1,
    },
    label: {
      color: '#007fff',
      fontSize: 16,
      fontWeight: 'bold',
    },
  });

  return (
    <Pressable style={styles.action} onPress={onPress}>
      <Text style={styles.label}>{label}</Text>
    </Pressable>
  );
};

export default FormButton;
