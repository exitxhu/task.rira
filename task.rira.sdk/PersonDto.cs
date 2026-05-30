using System.Runtime.Serialization;

namespace task.rira.sdk;

[DataContract]
public class PersonDto
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
    [DataMember(Order = 2)]
    public string FirstName { get; set; } = string.Empty;
    [DataMember(Order = 3)]
    public string LastName { get; set; } = string.Empty;
    [DataMember(Order = 4)]
    public string NationalId { get; set; } = string.Empty;
    [DataMember(Order = 5)]
    public DateTime BirthDate { get; set; }

}

[DataContract]
public class gRpcResponse
{
    [DataMember(Order = 1)]
    public bool IsSucessful { get; set; }
    [DataMember(Order = 2)]
    public string InternalMessage { get; set; } = string.Empty;
}
[DataContract]
public class gRpcResponse<T> : gRpcResponse
{
    [DataMember(Order = 10)]
    public T Value { get; set; }
}