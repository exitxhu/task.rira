using ProtoBuf.Grpc.Configuration;

namespace task.rira.sdk;

[Service("person")]
public interface IPersonService
{
    ValueTask<gRpcResponse> Delete(PersonDeleteRequestDto person);
    ValueTask<gRpcResponse> InsertSingle(PersonDto person);
    ValueTask<gRpcResponse> Update(PersonDto person);
    ValueTask<gRpcResponse<PersonDto?>> Get(PersonGetRequestDto person);
    ValueTask<gRpcResponse<PersonDto[]?>> Search(PersonSearchQueryDto person);
    ValueTask<gRpcResponse> InsertBatch(PersonDto[] people);
}
