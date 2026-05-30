using ProtoBuf.Grpc.Configuration;

namespace task.rira.sdk;

[Service("person")]
public interface IPersonService
{
    ValueTask<gRpcResponse> InsertSingle(PersonDto person);
    ValueTask<gRpcResponse> InsertBatch(PersonDto[] people);
}
