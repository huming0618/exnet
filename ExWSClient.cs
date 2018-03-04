using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace exnet
{
    class ExWSClient
    {
        readonly String PATH = "wss://real.okex.com:10441/websocket";
        public ExWSClient(){
            
        }

        public void Init(){
            this.Connect();
        }

        private async void Connect(){
            using(ClientWebSocket ws = new ClientWebSocket()){
                Uri pathUri = new Uri(PATH);
                await ws.ConnectAsync(pathUri, CancellationToken.None);
                if (ws.State == WebSocketState.Open){
                    Console.WriteLine("Connected to Server");
                    var msg = "{'event':'addChannel','channel':'ok_sub_spot_btm_usdt_depth'}";
                    ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
                    await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                
                    while(true){
                        Console.WriteLine("......");
                        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
                        Console.WriteLine("Receiving ....");
                        ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                        WebSocketReceiveResult result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                        Console.WriteLine(Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count));
                    }
                    
                }
                

            }
        }
    }
}
