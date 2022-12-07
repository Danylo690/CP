import React from "react";
import { View, Text,Button } from 'react-native';
import Server from "react-native-tcp-socket/lib/types/Server";
import {NativeRouter, Routes, Route} from 'react-router-native';
import ClientPage from "./ClientPage";
import HomePage from "./HomePage";
import ServerPage from "./ServerPage";
import GoBackButton from "./GoBackButton";
import {useLocation,useNavigate} from 'react-router-native';

const Router = () => {

  const location = useLocation();

  const isHome = location.pathname === '/';


  return (
<>
      {
        !isHome && <GoBackButton/>
      }
      <Routes>
        <Route index path="/" element={<HomePage />} />
        <Route path="/client" element={<ClientPage />}>
        </Route>

        <Route path="/server" element={<ServerPage />} />
      </Routes>
      </>
      
  )
 
};

export default Router;