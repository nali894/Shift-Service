namespace ExceptionsLib
{
    public enum ExceptionCode : int
    {
        [StringValue("Ok")]
        Ok = 0,
        [StringValue("New item created")]
        Created = 227,
        [StringValue("The service is accepted")]
        Accepted = 228,
        [StringValue("The service is rejected")]
        Rejected = 229,
        [StringValue("The service is canceled")]
        Canceled = 230,
        [StringValue("The service is not created")]
        IsNotCreated = 310,
        [StringValue("FAILURE:The service is not updated")]
        IsNotUpdated = 311,
        [StringValue("FAILURE: service has not been assigned any user")]
        IsNotAssigned = 312,
        [StringValue("FAILURE: Build failed with an exception")]
        Exception = 313
    }


}
