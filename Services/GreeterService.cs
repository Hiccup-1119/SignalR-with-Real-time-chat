using Grpc.Core;
using GrpcServer;              // your proto’s namespace

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest req, ServerCallContext ctx)
        => Task.FromResult(new HelloReply { Message = $"Hello, {req.Name}!" });
}
