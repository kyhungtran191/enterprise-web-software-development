import React, { createContext, useContext, useEffect, useRef, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { getAccessTokenFromLS } from '@/utils/auth';
import { useLocation } from 'react-router-dom';

function isEqual(obj1, obj2) {
  // If the objects are strictly equal, return true
  if (obj1 === obj2) {
    return true;
  }

  // If either object is not an object or is null, they're not equal
  if (typeof obj1 !== 'object' || typeof obj2 !== 'object' || obj1 === null || obj2 === null) {
    return false;
  }

  // Get the keys of the objects
  const keys1 = Object.keys(obj1);
  const keys2 = Object.keys(obj2);

  // If the number of keys is different, the objects are not equal
  if (keys1.length !== keys2.length) {
    return false;
  }

  // Compare each key and value pair recursively
  for (let key of keys1) {
    if (!isEqual(obj1[key], obj2[key])) {
      return false;
    }
  }

  // If all key and value pairs are equal, the objects are equal
  return true;
}

// Khởi tạo Context
const SignalRContext = createContext();

export const useSignalRContext = () => useContext(SignalRContext);

// SignalR Provider component
export const SignalRProvider = ({ children }) => {
  const [connections, setConnections] = useState({});

  const accessToken = getAccessTokenFromLS();
  const prevConnectionsRef = useRef({});


  // Hàm để thêm kết nối mới vào danh sách
  const addConnection = (connectionId, connection) => {
    setConnections(prevConnections => ({
      ...prevConnections,
      [connectionId]: connection
    }));
  };

  // Hàm để xóa kết nối khỏi danh sách
  const removeConnection = (connectionId) => {
    setConnections(prevConnections => {
      const { [connectionId]: _, ...updatedConnections } = prevConnections;
      return updatedConnections;
    });
  };


  useEffect(() => {
    if (accessToken && !connections["AnnouncementHub"]) {
      let connect = new HubConnectionBuilder()
        .withUrl("http://localhost:5272/hubs/announcement",
          { accessTokenFactory: () => accessToken })
        .withAutomaticReconnect()
        .build();

      addConnection('AnnouncementHub', connect);

    }

    if (accessToken && !connections["ChatHub"]) {
      let connect = new HubConnectionBuilder()
        .withUrl("http://localhost:5272/hubs/privatechat",
          { accessTokenFactory: () => accessToken })
        .withAutomaticReconnect()
        .build();

      addConnection('ChatHub', connect);
    }
  }, [accessToken, connections]);

  useEffect(() => {
    // Compare current connections with previous connections
    const connectionsChanged = !isEqual(prevConnectionsRef.current, connections);
    if (connectionsChanged) {
      // Update previous connections with current connections
      prevConnectionsRef.current = connections;
      // Start connections
      for (let [key, connect] of Object.entries(connections)) {
        connect?.start().then(() => {
          console.log(key + " successfully!");       
          
       
        }).catch((err) => console.log(err));
      }
    }
  }, [connections]);

  return (
    <SignalRContext.Provider value={{ connections }}>
      {children}
    </SignalRContext.Provider>
  );
};