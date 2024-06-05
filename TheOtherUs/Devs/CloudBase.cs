using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace TheOtherUs.Devs;

public abstract class CloudBase(string ip, int port) : IDisposable
{
    public IPAddress Address { get; init; } = IPAddress.Parse(ip);
    public int Port { get; init; } = port;
    public CloudManager.CloudInfo cloudInfo { get; init; }
    
    #nullable enable
    private IPEndPoint? _endPoint;
    #nullable disable
    
    public IPEndPoint EndPoint => _endPoint ??= new IPEndPoint(Address, Port);
    
    public virtual void Dispose()
    {
    }
}

public class HttpCloud(string ip, int port) : CloudBase(ip, port)
{
    public HttpClient client = new();

}

public class SocketCloud(string ip, int port) : CloudBase(ip, port)
{
    #nullable enable
    private Socket? _socket;
    #nullable disable

    public Socket socket => _socket ??= new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

}