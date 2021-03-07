namespace AWS.Insurance.Operations.Tests.Api.AConfig.Dtos
{
    public class ErrorDto
    {
        public ErrorMessageDto Errors { get; set; }
    }

    public class ErrorMessageDto
    {
        public string Message { get; set; }
    }
}
