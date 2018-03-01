using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace exnet
{
    class ExWSClient
    {
        readonly String PATH = "wss://real.okex.com:10441/websocket";
        public ExWSClient(){
            
        }

        public async void Init(){
            using(ClientWebSocket ws = new ClientWebSocket()){
                Uri pathUri = new Uri(PATH);
                await ws.ConnectAsync(pathUri, CancellationToken.None);


            }
        }
    }
}
