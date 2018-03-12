using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace exnet
{
    class ExWSClient
    {
        readonly String PATH = "wss://real.okex.com:10441/websocket";
        readonly String[] WATCH_LIST;

        public ExWSClient(){
            WATCH_LIST = new String[]{

                "{'event':'addChannel','channel':'ok_sub_spot_dadi_usdt_ticker'}",
                "{'event':'addChannel','channel':'ok_sub_spot_dadi_usdt_depth'}",
                // "{'event':'addChannel','channel':'ok_sub_spot_btc_usdt_depth_Y'}",
                "{'event':'addChannel','channel':'ok_sub_spot_dadi_usdt_deals'}",

                "{'event':'addChannel','channel':'ok_sub_spot_dadi_btc_ticker'}",
                "{'event':'addChannel','channel':'ok_sub_spot_dadi_btc_depth'}",
                // "{'event':'addChannel','channel':'ok_sub_spot_btc_usdt_depth_Y'}",
                "{'event':'addChannel','channel':'ok_sub_spot_dadi_btc_deals'}",

                "{'event':'addChannel','channel':'ok_sub_spot_dadi_eth_ticker'}",
                "{'event':'addChannel','channel':'ok_sub_spot_dadi_eth_depth'}",
                // "{'event':'addChannel','channel':'ok_sub_spot_btc_usdt_depth_Y'}",
                "{'event':'addChannel','channel':'ok_sub_spot_dadi_eth_deals'}"
            };
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

                    foreach (var msg in WATCH_LIST)
                    {
                        ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
                        await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                    }

                    while(true){
                        Console.WriteLine("......");
                        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
                        Console.WriteLine("Receiving ....");
                        ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                        WebSocketReceiveResult result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                        Console.WriteLine();

                        var response = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);

                        using (StreamWriter file = 
                            new StreamWriter(@"/tmp/ex.log", true))
                        {
                            file.WriteLine(response);
                        }
                        Console.WriteLine(response);
                    }
                    
                }
                

            }
        }
    }
}
