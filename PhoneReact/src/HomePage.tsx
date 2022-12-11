import {Button, SafeAreaView, StyleSheet, Pressable, Text} from 'react-native';
import React from 'react';
import {useNavigate} from 'react-router-native';
import {FontAwesomeIcon} from '@fortawesome/react-native-fontawesome';
import {faServer, faUser, faWifi} from '@fortawesome/free-solid-svg-icons';
const HomePage = () => {
  const navigator = useNavigate();

  return (
    <SafeAreaView
      style={{
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
      }}>
      <FontAwesomeIcon icon={faWifi} size={75} color="#007fff" />
      <Text style={{fontSize: 30, fontWeight: 'bold', marginBottom: 20}}>
        Welcome to the app
      </Text>

      <Pressable style={styles.button} onPress={() => navigator('/client')}>
        <FontAwesomeIcon icon={faUser} size={30} color="white" />
        <Text style={styles.text}>Being client</Text>
      </Pressable>

      <Pressable style={styles.button} onPress={() => navigator('/server')}>
        <FontAwesomeIcon icon={faServer} size={30} color="white" />
        <Text style={styles.text}>Being server</Text>
      </Pressable>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  button: {
    backgroundColor: '#007fff',
    paddingHorizontal: 30,
    paddingVertical: 15,
    borderRadius: 10,
    width: '75%',
    margin: 20,
    elevation: 10,
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'space-evenly',
    alignItems: 'center',
  },
  text: {
    color: 'white',
    fontSize: 20,
    textAlign: 'center',
  },
});

export default HomePage;
