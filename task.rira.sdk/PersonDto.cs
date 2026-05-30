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
public class PersonSearchQueryDto
{
    [DataMember(Order = 1)]
    public string? FirstName { get; set; } = string.Empty;
    [DataMember(Order = 3)]
    public string? LastName { get; set; } = string.Empty;
    [DataMember(Order = 4)]
    public string? NationalId { get; set; } = string.Empty;
    [DataMember(Order = 5)]
    public DateTime? BirthDateFrom { get; set; } = null;
    [DataMember(Order = 6)]
    public DateTime? BirthDateTo { get; set; } = null;
    [DataMember(Order = 7)]
    public int Page { get; set; } = 1;
    [DataMember(Order = 8)]
    public int Size { get; set; } = 10;


}
[DataContract]
public class PersonDeleteRequestDto
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
}
[DataContract]
public class PersonGetRequestDto
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
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