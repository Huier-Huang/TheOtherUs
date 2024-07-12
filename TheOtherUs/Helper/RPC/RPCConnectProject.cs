using System;

namespace TheOtherUs.Helper.RPC;

#nullable enable
public class RPCConnectProject : IDisposable
{
    public CustomRPC rpc { get; init; }
    public Action<FastRpcWriter>? PreWrite { get; set; }

    private FastRpcWriter? _writer;

    public string Id { get; private set; } = $"{nameof(RPCConnectProject)} x";
    
    public bool Started { get; private set; }
        
    public static RPCConnectProject startNewProject(CustomRPC rpc, string Id, Action<FastRpcWriter>? PreWrite = null)
    {
        return new RPCConnectProject
        {
            rpc = rpc,
            PreWrite = PreWrite,
            Id = Id
        };
    }

    public void Update(Action<FastRpcWriter> writer)
    {
        if (!Started) return;
        _writer ??= FastRpcWriter.StartNewRpcWriter(rpc);
        writer.Invoke(_writer);
    }

    public void End()
    {
        Started = false;
    }

    public void Start()
    {
        Started = true;
        _writer ??= FastRpcWriter.StartNewRpcWriter(rpc);
    }

    public void Dispose()
    {
        End();
    }
}