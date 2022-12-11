import React from 'react';
import {View, Text, TextInput, StyleSheet} from 'react-native';

type Props = {
  label: string;
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
};

export const FormInput = ({label, value, onChange, placeholder}: Props) => {
  return (
    <View style={styles.container}>
      <Text style={styles.label}>{label}</Text>
      <TextInput
        value={value}
        onChangeText={onChange}
        placeholder={placeholder}
        style={styles.input}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginVertical: 10,
  },
  label: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#000',
    width: '30%',
  },
  input: {
    width: '60%',
    height: 40,
    borderWidth: 1,
    borderColor: '#000',
    paddingHorizontal: 10,
  },
});

export default FormInput;
