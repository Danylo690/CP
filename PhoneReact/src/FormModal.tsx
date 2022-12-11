import React from 'react';
import {
  View,
  Text,
  Modal,
  Pressable,
  Switch,
  Alert,
  TextInput,
  StyleSheet,
  FlatList,
} from 'react-native';
import FormButton from './FormButton';
import FormInput from './FormInput';
import FormSwitch from './FormSwitch';
import {useAtom} from 'jotai';
import {computersAtom} from './store';
import type {Computer} from './store';
type Props = {
  isOpen: boolean;
  onClose: (isOpen: boolean) => void;
};

const FormModal = ({isOpen, onClose}: Props) => {
  const [computerStore, setComputerStore] = useAtom(computersAtom);

  const [isAnon, setIsAnon] = React.useState(false);

  const [hostname, setHostname] = React.useState('');
  const [ipAddress, setIpAddress] = React.useState('');
  const [username, setUsername] = React.useState('');
  const [password, setPassword] = React.useState('');

  const handleCloseModal = () => {
    onClose(false);
  };

  const handleSwitch = () => {
    setIsAnon(x => !x);
  };

  const handleSubmit = () => {
    const newComputer: Computer = {
      id: computerStore.computers.length + 1,
      hostname,
      ip: ipAddress,
      username,
      password,
      isAnonymous: isAnon,
    };

    setComputerStore(prev => ({
      ...prev,
      computers: [...prev.computers, newComputer],
    }));

    handleCloseModal();
  };

  return (
    <Modal
      animationType="fade"
      transparent={true}
      visible={isOpen}
      onRequestClose={() => {
        Alert.alert('Modal has been closed.');
        handleCloseModal();
      }}>
      <View style={styles.modal}>
        <View style={styles.form}>
          <Text style={styles.header}>Server</Text>

          <FormInput
            label="Hostname"
            placeholder="Any name for new server"
            value={hostname}
            onChange={text => setHostname(text)}
          />

          <FormInput
            label="Ip address"
            placeholder=":192.168.1.100"
            value={ipAddress}
            onChange={text => setIpAddress(text)}
          />

          <FormInput
            label="Username"
            placeholder="Enter username"
            value={username}
            onChange={text => setUsername(text)}
          />

          <FormInput
            label="Password"
            placeholder="Enter password"
            value={password}
            onChange={text => setPassword(text)}
          />

          <FormSwitch
            label="Anonymous"
            value={isAnon}
            onChange={handleSwitch}
          />

          <View style={styles.actions}>
            <FormButton label="Cancel" onPress={handleCloseModal} id={0} />
            <FormButton label="Ok" onPress={handleSubmit} id={1} />
          </View>
        </View>
      </View>
    </Modal>
  );
};

const styles = StyleSheet.create({
  header: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
  },

  modal: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    height: '100%',
    width: '100%',
    backgroundColor: '#00000088',
  },
  form: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    backgroundColor: '#ddd',
    borderRadius: 20,
    overflow: 'hidden',
    elevation: 5,
    width: '90%',
  },
  actions: {
    display: 'flex',
    justifyContent: 'center',
    flexDirection: 'row',
    width: '100%',
  },
});

export default FormModal;
