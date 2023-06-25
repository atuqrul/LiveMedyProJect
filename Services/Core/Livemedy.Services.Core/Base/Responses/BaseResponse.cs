namespace Livemedy.Core.Base.Responses;

public class BaseResponse
{
    public int Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDelete { get; set; }

}
