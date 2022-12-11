import React from 'react';

import {View, Text, Switch, StyleSheet} from 'react-native';

type Props = {
  label: string;
  value: boolean;
  onChange: (value: boolean) => void;
};

const FormSwitch = ({label, value, onChange}: Props) => {
  return (
    <View style={styles.switchContainer}>
      <Text style={styles.label}>Anonymous</Text>
      <View style={styles.switchWrap}>
        <Switch
          style={styles.switch}
          trackColor={{false: '#767577', true: '#81b0ff'}}
          thumbColor={value ? '#f5dd4b' : '#f4f3f4'}
          onValueChange={onChange}
          value={value}
        />
      </View>
    </View>
  );
};

export default FormSwitch;

const styles = StyleSheet.create({
  switchContainer: {
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
  switchWrap: {
    marginLeft: 'auto',
    width: '60%',
    justifyContent: 'flex-start',
    alignItems: 'center',
    display: 'flex',
    flexDirection: 'row',
  },
  switch: {
    width: 50,
  },
});
