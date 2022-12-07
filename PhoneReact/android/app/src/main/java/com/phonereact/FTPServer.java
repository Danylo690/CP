package com.phonereact;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.facebook.react.bridge.Callback;
import com.facebook.react.bridge.ReactApplicationContext;
import com.facebook.react.bridge.ReactContextBaseJavaModule;
import com.facebook.react.bridge.ReactMethod;

import android.Manifest;
import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.util.Log;

import org.apache.ftpserver.DataConnectionConfigurationFactory;
import org.apache.ftpserver.ConnectionConfigFactory;
import org.apache.ftpserver.FtpServer;
import org.apache.ftpserver.FtpServerFactory;
import org.apache.ftpserver.ftplet.Authority;
import org.apache.ftpserver.ftplet.FtpException;
import org.apache.ftpserver.ftplet.FtpReply;
import org.apache.ftpserver.ftplet.FtpRequest;
import org.apache.ftpserver.ftplet.FtpSession;
import org.apache.ftpserver.ftplet.Ftplet;
import org.apache.ftpserver.ftplet.FtpletContext;
import org.apache.ftpserver.ftplet.FtpletResult;
import org.apache.ftpserver.ftplet.UserManager;
import org.apache.ftpserver.listener.ListenerFactory;
import org.apache.ftpserver.usermanager.impl.BaseUser;
import org.apache.ftpserver.usermanager.impl.WritePermission;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import android.os.Environment;
import android.content.Intent;
import android.provider.Settings;
import android.net.Uri;

public class FTPServer extends ReactContextBaseJavaModule {
    public FTPServer(@Nullable ReactApplicationContext reactContext) {
        super(reactContext);
    }

    @NonNull
    @Override
    public String getName() {
        return "FTPServer";
    }

    @ReactMethod
    public void StartFTPServer(){
        try{
            FtpServerFactory serverFactory = new FtpServerFactory();
            ListenerFactory factory = new ListenerFactory();

            factory.setPort(2121);
            serverFactory.addListener("default", factory.createListener());

            ConnectionConfigFactory connectionConfigFactory = new ConnectionConfigFactory();
            connectionConfigFactory.setAnonymousLoginEnabled(false);

            BaseUser user = new BaseUser(); 
            user.setName("anonymous");
            user.setHomeDirectory(Environment.getExternalStorageDirectory().getPath());

            List<Authority> authorities = new ArrayList<Authority>();  
            authorities.add(new WritePermission());
            user.setAuthorities(authorities); 

            serverFactory.getUserManager().save(user); 

            FtpServer server = serverFactory.createServer();  
            server.start();
        }
        catch (Exception ex) {

        } 
    }
}