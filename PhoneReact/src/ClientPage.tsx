import {faBots} from '@fortawesome/free-brands-svg-icons';
import React from 'react';
import {
  Text,
  Pressable,
  View,
  Modal,
  Alert,
  StyleSheet,
  TextInput,
  Switch,
  FlatList,
} from 'react-native';
import FormModal from './FormModal';
import {useAtom} from 'jotai';
import {computersAtom} from './store';
import ClientHeader from './ClientHeader';
import ComputerListItem from './ComputerListItem';
const ClientPage = () => {
  const [hostNameText, setHostNameText] = React.useState('');
  const [hostAddress, setHostAddress] = React.useState('');
  const [usernameText, setUsernameText] = React.useState('');
  const [passwordText, setPasswordText] = React.useState('');

  const [isEnabled, setIsEnabled] = React.useState(false);
  const toggleSwitch = () => setIsEnabled(previousState => !previousState);

  const [isModalOpen, setIsModalOpen] = React.useState(false);

  const [computersStore, setComputersStore] = useAtom(computersAtom);

  const openModal = () => {
    setIsModalOpen(true);
  };

  const computers = computersStore.computers;

  return (
    <>
      {isModalOpen && (
        <FormModal isOpen={isModalOpen} onClose={setIsModalOpen} />
      )}

      <ClientHeader onCreate={openModal} />

      <FlatList
        data={computers}
        renderItem={({item}) => (
          <ComputerListItem label={item.hostname} {...item} />
        )}
      />
    </>
  );
};
const styles = StyleSheet.create({});
export default ClientPage;
