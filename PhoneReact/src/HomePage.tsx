import {Button,SafeAreaView} from 'react-native';
import React from 'react';
import {useNavigate} from 'react-router-native';



const HomePage = ()=>{
  const navigator = useNavigate();



  return(<SafeAreaView
    style={{
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
    }}
  >
    
    <Button
        title="Being client"
        color="#841584"
        onPress={() => navigator('/client')}
      />

      <Button
        title="Start ftp server"
        color="#841584"
        onPress={() => navigator('/server')}
      />
  </SafeAreaView>)
}

export default HomePage;