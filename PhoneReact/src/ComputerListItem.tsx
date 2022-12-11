import React from 'react';
import {Pressable, Text, StyleSheet, View} from 'react-native';
import {faDesktop} from '@fortawesome/free-solid-svg-icons';
import {FontAwesomeIcon} from '@fortawesome/react-native-fontawesome';
import type {Computer} from './store';
interface Props extends Computer {
  label: string;
}
const ComputerListItem = ({label, ip, username, password}: Props) => {
  const handlePress = () => {
    // Open the computer
  };

  return (
    <Pressable style={styles.container} onPress={handlePress}>
      <View style={styles.icon}>
        <FontAwesomeIcon icon={faDesktop} />
      </View>
      <Text style={styles.text}>{label}</Text>
    </Pressable>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    // justifyContent: 'space-between',
    flexDirection: 'row',
    width: '100%',
    height: 50,
    borderTopWidth: 1,
    borderBottomWidth: 1,
    borderColor: '#000',
    padding: 10,
  },
  icon: {
    marginRight: 20,
  },
  text: {
    fontSize: 20,
    fontWeight: 'bold',
  },
});

export default ComputerListItem;
