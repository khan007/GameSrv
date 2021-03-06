﻿using RandM.RMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandM.GameSrv {
    class WebSocketServerThread : ServerThread {
        public WebSocketServerThread() : base() {
            _ConnectionType = ConnectionType.WebSocket;
            _LocalAddress = Config.Instance.WebSocketServerIP;
            _LocalPort = Config.Instance.WebSocketServerPort;
        }

        protected override void HandleNewConnection(TcpConnection newConnection) {
            if (newConnection == null) {
                throw new ArgumentNullException("newConnection");
            }

            WebSocketConnection TypedConnection = new WebSocketConnection();
            if (TypedConnection.Open(newConnection.GetSocket())) {
                // TODOX Start a proxy thread instead of a clientthread
                ClientThread NewClientThread = new ClientThread(TypedConnection, _ConnectionType);
                NewClientThread.Start();
            } else {
                RMLog.Info("No carrier detected (probably a portscanner)");
                TypedConnection.Close();
            }
        }
    }
}
