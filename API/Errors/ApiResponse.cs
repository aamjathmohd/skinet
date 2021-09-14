using System;


namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode,string message=null)
        {
            StatusCode=statusCode;
            Message=message ?? GetDefaultMessageForStatuscode(statusCode);
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        // public int MyProperty { get; set; }
        private string GetDefaultMessageForStatuscode(int statusCode)
        {
           return statusCode switch
           {
               400=> "A bad request you have made",
               401=>"Authorized, you are not",
               404=>"Resource found, it was not",
               500=> "Errors are the path to dark side. Errors leads to anger. Anger leads to hate. Hate leads to carrier change",
               _=>null
           };
        }

        
    }
}